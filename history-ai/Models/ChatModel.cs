namespace HistoryAI.Models
{
    public class ChatModel
    {
        public class ChatRequest
        {
            public string UserMessage { get; set; } = string.Empty;
            public List<ChatMessage> History { get; set; } = new();
        }

        public class ChatMessage
        {
            public string Role { get; set; } = string.Empty;
            public string Content { get; set; } = string.Empty;
        }

        public class ChatResponse
        {
            public string Message { get; set; } = string.Empty;
            public bool IsSuccess { get; set; }
        }


        public class AIRequest
        {
            //public string Model { get; set; } = string.Empty;
            public List<AIMessage> Messages { get; set; } = new();
            public int MaxTokens { get; set; } = 1024;
        }

        public class AIMessage
        {
            public string Role { get; set; } = string.Empty;
            public string Content { get; set; } = string.Empty;
        }

        public class AIResponse
        {
            public List<AIChoice> Choices { get; set;  } = new();
        }

        public class AIChoice
        {
            public AIMessage Message { get; set; } = new();
            public string FinishReason { get; set; } = string.Empty;
            public int Index { get; set; }
        }

    }
}
