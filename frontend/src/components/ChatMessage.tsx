import React from 'react';
import { Message } from '../types';

interface ChatMessageProps {
  message: Message;
  onCopy: (content: string) => void;
  onRegenerate: () => void;
}

const ChatMessage: React.FC<ChatMessageProps> = ({ message, onCopy, onRegenerate }) => {
  const formatTime = (date: Date) => {
    return new Date(date).toLocaleTimeString('en-US', {
      hour: '2-digit',
      minute: '2-digit',
    });
  };

  return (
    <div
      className={`flex mb-6 animate-fadeIn ${
        message.isUser ? 'justify-end' : 'justify-start'
      }`}
    >
      <div className={`max-w-[70%] ${message.isUser ? 'items-end' : 'items-start'} flex flex-col`}>
        {/* Message Bubble */}
        <div
          className={`px-4 py-3.5 rounded-2xl text-[15px] leading-relaxed ${
            message.isUser
              ? 'bg-primary text-white rounded-br-sm'
              : 'bg-white text-slate-900 border border-slate-200 rounded-bl-sm'
          }`}
        >
          {message.content}
        </div>

        {/* Metadata (AI only) */}
        {!message.isUser && (
          <>
            <div className="flex items-center gap-2 mt-2 text-xs text-slate-400">
              {message.source && (
                <span className="px-2.5 py-1 bg-slate-50 rounded-full text-slate-600 font-medium">
                  {message.source} {message.confidence && `${Math.round(message.confidence * 100)}%`}
                </span>
              )}
              <span>{formatTime(message.timestamp)}</span>
            </div>

            {/* Actions */}
            <div className="flex gap-2 mt-2">
              <button
                onClick={() => onCopy(message.content)}
                className="px-3 py-1.5 text-xs border border-slate-200 rounded-lg text-slate-600 hover:bg-slate-50 hover:border-primary hover:text-primary transition-all"
              >
                📋 Copy
              </button>
              <button
                onClick={onRegenerate}
                className="px-3 py-1.5 text-xs border border-slate-200 rounded-lg text-slate-600 hover:bg-slate-50 hover:border-primary hover:text-primary transition-all"
              >
                🔄 Regenerate
              </button>
            </div>
          </>
        )}
      </div>
    </div>
  );
};

export default ChatMessage;
