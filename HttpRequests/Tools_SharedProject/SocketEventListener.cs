using System.Diagnostics.Tracing; //for EventListener

namespace Tools_SharedProject
{
    class SocketEventListener : EventListener //class for catching different events of another classes or Windows components
    {
        public TaskCompletionSource<bool> SocketClosedTcs = new TaskCompletionSource<bool>();

        protected override void OnEventSourceCreated(EventSource eventSource) //method called when a new EventSource is created
        {

            if (eventSource.Name == "System.Net.Sockets") //Check event source name, to listen only to Sockets event source events
            {
                Console.WriteLine($"Detected EventSource===: {eventSource.Name}");
                EnableEvents(eventSource, EventLevel.LogAlways);  //enable listening to all events from this source
            }

        }
        protected override void OnEventWritten(EventWrittenEventArgs eventData) //method called when an event is written to enabled EvenetSource
        {
            Console.WriteLine($"Source===: {eventData.EventSource.Name}, Event===: {eventData.EventName}");
            if (eventData.Payload != null)
            {
                for (int i = 0; i < eventData.Payload.Count; i++)
                {
                    Console.WriteLine($"  {eventData.PayloadNames[i]}===: {eventData.Payload[i]}");
                }

                if (eventData.EventName == "ConnectionClosed" || eventData.EventName == "DisconnectStop" || eventData.EventName == "ConnectFailed")
                {
                    Console.WriteLine($"Socket closed: {eventData.Payload?[0]}");
                    SocketClosedTcs.TrySetResult(true); // сигнализируем TaskCompletionSource
                }
            }
            Console.WriteLine();
        }

    }
}
