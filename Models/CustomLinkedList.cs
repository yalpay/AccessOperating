namespace AccessOperating.Models
{
    // Generic implementation of LinkedLists
    public class CustomLinkedList<T>
    {
        public class Node
        {
            public T Value { get; }
            public Node Next { get; set; }

            public Node(T value)
            {
                Value = value;
                Next = null;
            }
        }

        public Node head;
        public Node tail;

        public Node GetHead()
        {
            return head;
        }
        public int Count { get; private set; }

        public void AddLast(T value)
        {
            var newNode = new Node(value);

            if (head == null)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                tail.Next = newNode;
                tail = newNode;
            }

            Count++;
        }
    }

}
