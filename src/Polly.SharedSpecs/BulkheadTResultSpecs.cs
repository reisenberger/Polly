using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Polly.Specs
{
    public class BulkheadTResultSpecs
    {



        [Fact]
        public void Should_initial_Bulkhead_with_processor_count_on_empty_parameters_syntax()
        {
            Policy<int> policy = Policy
                                      .Bulkhead<int>();

            var type = policy.GetType();
            type.Name.Should().Be("BulkheadPolicy`1");
        }


        [Fact]
        public void Should_throw_when_maxparallelization_less_or_equal_to_zero()
        {
            Action policy = () => Policy
                                      .Bulkhead<int>(0, 1);

            policy.ShouldThrow<ArgumentOutOfRangeException>().And
                  .ParamName.Should().Be("maxParallelization");
        }

        [Fact]
        public void Should_throw_when_maxqueuedactions_less_than_zero()
        {
            Action policy = () => Policy
                                      .Bulkhead<int>(1, -1);

            policy.ShouldThrow<ArgumentOutOfRangeException>().And
                  .ParamName.Should().Be("maxQueuedActions");
        }

        [Fact]
        public void Should_throw_when_maxqueuedactions_less_than_maxparallelization()
        {
            Action policy = () => Policy
                                      .Bulkhead<int>(3, 1);

            policy.ShouldThrow<ArgumentOutOfRangeException>().And
                  .ParamName.Should().Be("maxQueuedActions");
        }
        
        [Fact]
        public void Should_not_disurpt_throttling_while_cancelling()
        {
            var maxParallelization = 1;
            Policy<int> policy = Policy
                                        .Bulkhead<int>(maxParallelization, maxParallelization + 5);
            
            var tokens = new CancellationToken[maxParallelization + 5];


            for (var i = 0; i < maxParallelization + 5; i++)
            {
                var cts = new CancellationTokenSource();
                tokens[i] = cts.Token;
                var result = policy.ExecuteAndCapture((ct) =>
                {
                    Interlocked.Increment(ref count);
                    if (_indexCount == 4)
                    {
                        cts.Cancel();
                        Interlocked.Increment(ref _indexCount);
                        ct.ThrowIfCancellationRequested();
                    }
                    Interlocked.Increment(ref _indexCount);
                    Polly.Utilities.SystemClock.SleepAsync(100.Milliseconds(), ct);
                    count.Should().BeLessOrEqualTo(maxParallelization);
                    Interlocked.Decrement(ref count);
                    return 0;
                }, tokens[i]);
                if (i == 4) tokens[4].IsCancellationRequested.Should().Be(true);
            }
            
            tokens[4].IsCancellationRequested.Should().Be(true);
            _indexCount.Should().Be(6);

        }

        static int count;
        static int _indexCount;

    }
}
