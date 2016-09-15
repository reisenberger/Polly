using FluentAssertions;
using Polly.Bulkhead;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
#if !PORTABLE
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif

namespace Polly.Specs
{
    public class BulkheadSpecs
    {
        [Fact]
        public void Should_throw_when_maxparallelization_less_or_equal_to_zero()
        {
            Action policy = () => Policy
                                      .Bulkhead(0, 1);

            policy.ShouldThrow<ArgumentOutOfRangeException>().And
                  .ParamName.Should().Be("maxParallelization");
        }

        [Fact]
        public void Should_throw_when_maxqueuedactions_less_than_zero()
        {
            Action policy = () => Policy
                                      .Bulkhead(1, -1);

            policy.ShouldThrow<ArgumentOutOfRangeException>().And
                  .ParamName.Should().Be("maxQueuedActions");
        }

        [Fact]
        public void Should_throw_when_maxqueuedactions_less_than_maxparallelization()
        {
            Action policy = () => Policy
                                      .Bulkhead(3, 1);

            policy.ShouldThrow<ArgumentOutOfRangeException>().And
                  .ParamName.Should().Be("maxQueuedActions");
        }

        [Fact]
        public void Should_initial_Bulkhead_with_processor_count_on_empty_parameters_syntax()
        {
            Policy policy = Policy
                                      .Bulkhead();
            
            var type = policy.GetType();
            type.Name.Should().Be("BulkheadPolicy");
        }


        [Fact]
        public void Should_have_only_one_thread_at_the_time()
        {
            count = 0;
            _indexCount = 0;
            for (var maxParallelization = 1; maxParallelization < 10; maxParallelization++)
            {
                Policy policy = Policy
                                          .Bulkhead(maxParallelization, maxParallelization + 2);

                var tasks = new Task[maxParallelization + 2];

                for (var i = 0; i < maxParallelization + 2; i++)
                {
                    tasks[i] = Task.Run(() =>
                    {
                        policy.Execute(() =>
                        {
                            Interlocked.Increment(ref count);
                            Polly.Utilities.SystemClock.Sleep(100.Milliseconds(), CancellationToken.None);
                            count.Should().BeLessOrEqualTo(maxParallelization);
                            Interlocked.Decrement(ref count);
                        });
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
                                        .Bulkhead(maxParallelization, maxParallelization + 5);
            
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
                    Polly.Utilities.SystemClock.Sleep(100.Milliseconds(), ct);
                    count.Should().BeLessOrEqualTo(maxParallelization);
                    Interlocked.Decrement(ref count);
                }, tokens[i]);
            }
            
            tokens[4].IsCancellationRequested.Should().Be(true);
            _indexCount.Should().Be(6);

        }

        static int count;
        static int _indexCount;

        [Fact]
        public void Should_serialize_semaphore_rejected_exception()
        {
            SemaphoreRejectedException ex = new SemaphoreRejectedException();
#if !PORTABLE
            // Round-trip the exception: Serialize and de-serialize with a BinaryFormatter
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                // "Save" object state
                bf.Serialize(ms, ex);

                // Re-use the same stream for de-serialization
                ms.Seek(0, 0);

                // Replace the original exception with de-serialized one
                ex = (SemaphoreRejectedException)bf.Deserialize(ms);
            }
#endif
        }

        [Fact]
        public void Should_serialize_semaphore_rejected_exception_with_inner_exception()
        {
            SemaphoreRejectedException ex = new SemaphoreRejectedException("This is a test", new Exception());
#if !PORTABLE
            // Round-trip the exception: Serialize and de-serialize with a BinaryFormatter
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                // "Save" object state
                bf.Serialize(ms, ex);

                // Re-use the same stream for de-serialization
                ms.Seek(0, 0);

                // Replace the original exception with de-serialized one
                ex = (SemaphoreRejectedException)bf.Deserialize(ms);
            }
#endif
        }



    }
}
