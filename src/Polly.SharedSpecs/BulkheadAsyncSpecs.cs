using FluentAssertions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Polly.Specs
{
    /// <summary>
    /// 
    /// </summary>
    public class BulkheadAsyncSpecs
    {
        
        [Fact]
        public void Should_throw_when_maxparallelization_less_or_equal_to_zero()
        {
            Action policy = () => Policy
                                      .BulkheadAsync(0, 1);

            policy.ShouldThrow<ArgumentOutOfRangeException>().And
                  .ParamName.Should().Be("maxParallelization");
        }
        

        [Fact]
        public void Should_throw_when_maxqueuedactions_less_than_zero()
        {
            Action policy = () => Policy
                                      .BulkheadAsync(1, -1);

            policy.ShouldThrow<ArgumentOutOfRangeException>().And
                  .ParamName.Should().Be("maxQueuedActions");
        }

        [Fact]
        public void Should_throw_when_maxqueuedactions_more_than_maxParallelization()
        {
            Action policy = () => Policy
                                      .BulkheadAsync(2, 1);

            policy.ShouldThrow<ArgumentOutOfRangeException>().And
                  .ParamName.Should().Be("maxQueuedActions");
        }




        [Fact]
        public void Should_initial_Bulkhead_with_processor_count_on_empty_parameters_syntax()
        {
            Policy policy = Policy
                                      .BulkheadAsync();

            var type = policy.GetType();
            type.Name.Should().Be("BulkheadPolicy");
        }

        //[Fact]
        public void Should_throw_when_exeeding_maxQueuedActions()
        {
            count = 0;
            _indexCount = 0;
            for (var l = 1; l < 10; l++)
            {
                var maxParallelization = 1;

                Policy policy = Policy
                                          .BulkheadAsync(maxParallelization, maxParallelization + 2);

                var tasks = new Task[maxParallelization + 20];

                for (var i = 0; i < maxParallelization + 20; i++)
                {
                    tasks[i] = policy.ExecuteAndCaptureAsync(async () =>
                        {
                            Interlocked.Increment(ref count);
                            await Polly.Utilities.SystemClock.SleepAsync(1.Milliseconds(), CancellationToken.None);
                            count.Should().BeLessOrEqualTo(maxParallelization);
                            Interlocked.Decrement(ref count);
                        });
                }

                Task.WaitAll(tasks);

                var exceptionCount = tasks.Count(x =>
                    {
                        var task = x as Task<PolicyResult>;
                        return task.Result.Outcome == OutcomeType.Failure;
                    }
                );

                exceptionCount.Should().BeInRange(16, 17);
            }
        }


        [Fact]
        public void Should_have_only_one_thread_at_the_time()
        {
            count = 0;
            _indexCount = 0;
            for (var maxParallelization = 1; maxParallelization < 10; maxParallelization++)
            {
                Policy policy = Policy
                                          .BulkheadAsync(maxParallelization, maxParallelization + 2);

                var tasks = new Task[maxParallelization + 2];

                for (var i = 0; i < maxParallelization + 2; i++)
                {
                    tasks[i] = policy.ExecuteAsync(async () =>
                    {
                        Interlocked.Increment(ref count);
                        await Polly.Utilities.SystemClock.SleepAsync(100.Milliseconds(), CancellationToken.None);
                        count.Should().BeLessOrEqualTo(maxParallelization);
                        Interlocked.Decrement(ref count);
                    });
                }

                Task.WaitAll(tasks);
            }
        }

        [Fact]
        public void Should_not_disurpt_throttling_while_cancelling()
        {
            count = 0;
            _indexCount = 0;
            var maxParallelization = 1;
            Policy policy = Policy
                                        .BulkheadAsync(maxParallelization, maxParallelization + 5);

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
