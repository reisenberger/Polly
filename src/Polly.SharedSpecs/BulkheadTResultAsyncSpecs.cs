using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Polly.Specs
{
    /// <summary>
    /// 
    /// </summary>
    public class BulkheadTResultAsyncSpecs
    {
        
        [Fact]
        public void Should_throw_when_maxparallelization_less_or_equal_to_zero()
        {
            Action policy = () => Policy
                                      .BulkheadAsync<int>(0, 1);

            policy.ShouldThrow<ArgumentOutOfRangeException>().And
                  .ParamName.Should().Be("maxParallelization");
        }
        

        [Fact]
        public void Should_throw_when_maxqueuedactions_less_than_zero()
        {
            Action policy = () => Policy
                                      .BulkheadAsync<int>(1, -1);

            policy.ShouldThrow<ArgumentOutOfRangeException>().And
                  .ParamName.Should().Be("maxQueuedActions");
        }

        [Fact]
        public void Should_throw_when_maxqueuedactions_more_than_maxParallelization()
        {
            Action policy = () => Policy
                                      .BulkheadAsync<int>(2, 1);

            policy.ShouldThrow<ArgumentOutOfRangeException>().And
                  .ParamName.Should().Be("maxQueuedActions");
        }



        [Fact]
        public void Should_initial_Bulkhead_with_processor_count_on_empty_parameters_syntax()
        {
            Policy<int> policy = Policy
                                      .BulkheadAsync<int>();

            var type = policy.GetType();
            type.Name.Should().Be("BulkheadPolicy`1");
        }
        
        [Fact]
        public void Should_not_disurpt_throttling_while_cancelling()
        {
            var maxParallelization = 1;
            Policy<int> policy = Policy
                                        .BulkheadAsync<int>(maxParallelization, maxParallelization + 5);

            var tasks = new Task[maxParallelization + 5];

            var tokens = new CancellationToken[maxParallelization + 5];


            for (var i = 0; i < maxParallelization + 5; i++)
            {
                var cts = new CancellationTokenSource();
                tokens[i] = cts.Token;
                tasks[i] = policy.ExecuteAndCaptureAsync(async (ct) =>
                {
                    Interlocked.Increment(ref count);
                    if (_indexCount == 4)
                    {
                        cts.Cancel();
                        Interlocked.Increment(ref _indexCount);
                        ct.ThrowIfCancellationRequested();
                    }
                    Interlocked.Increment(ref _indexCount);
                    await Polly.Utilities.SystemClock.SleepAsync(100.Milliseconds(), ct);
                    count.Should().BeLessOrEqualTo(maxParallelization);
                    Interlocked.Decrement(ref count);
                    return await Task.FromResult(0);
                }, tokens[i], false);
            }

            Task.WaitAll(tasks);
            tokens[4].IsCancellationRequested.Should().Be(true);
            _indexCount.Should().Be(6);

        }
        
        static int count;
        static int _indexCount;

    }
}
