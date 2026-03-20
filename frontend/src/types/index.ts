export interface Message {
  id: string;
  content: string;
  isUser: boolean;
  timestamp: Date;
  source?: string;
  confidence?: number;
}

export interface ChatResponse {
  answer: string;
  source: string;
  confidence: number;
  intent: string;
}
