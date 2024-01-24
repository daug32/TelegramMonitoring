using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Monitoring.Core.Implementation.Notificators.Models
{
    public class SafeConcurrentQueue<T>
        where T : class
    {
        private readonly ConcurrentQueue<T> _queue;

        public SafeConcurrentQueue( IEnumerable<T> queue )
        {
            _queue = new ConcurrentQueue<T>( queue );
        }

        public bool IsEmpty => _queue.IsEmpty;

        public T Pop()
        {
            while ( true )
            {
                if ( IsEmpty )
                {
                    return null;
                }
                
                if ( !_queue.TryDequeue( out T result ) )
                {
                    continue;
                }

                return result;
            }
        }

        public void Push( T item )
        {
            _queue.Enqueue( item );
        }
    }
}