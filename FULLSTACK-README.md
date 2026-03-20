# 🚀 FundAI - Full Stack Application

## 📋 Overview

**FundAI** is a premium AI-powered financial assistant with:
- **Backend**: ASP.NET Core 9.0 + Qdrant Vector DB
- **Frontend**: React 19 + TypeScript + Tailwind CSS
- **Features**: Chat interface + Analytics Dashboard

---

## ⚡ Quick Start (One Command)

### Option 1: Double-Click Startup
```
Double-click: START-FULLSTACK.bat
```

This will:
1. Start Backend API on `https://localhost:44328`
2. Start Frontend on `http://localhost:3000`
3. Auto-open browser

---

## 🛠️ Manual Setup

### Step 1: Install Frontend Dependencies
```bash
cd d:\AI_Fund\Ai_Fund\frontend
npm install
```

### Step 2: Start Backend
```bash
cd d:\AI_Fund\Ai_Fund\Ai_Fund
dotnet run
```

### Step 3: Start Frontend
```bash
cd d:\AI_Fund\Ai_Fund\frontend
npm run dev
```

---

## 🎨 Features

### ✅ Chat Interface
- Real-time AI conversations
- Message history
- Copy & regenerate responses
- Typing indicators
- Suggestion chips
- Empty state with quick questions

### ✅ Professional Dashboard
- **Dynamic Stats Cards**
  - Total Queries (live count)
  - Knowledge Gaps (auto-updated)
  - Average Confidence (calculated)
  - Active Users (tracked)

- **Knowledge Gaps Table**
  - Real-time data from API
  - Search functionality
  - Status badges (New, Reviewing, Resolved)
  - Sortable columns
  - Action buttons

- **Auto-Refresh**
  - Updates every 30 seconds
  - No manual refresh needed

### ✅ Navigation
- Seamless switching between Chat & Dashboard
- Header with logo and user menu
- Responsive design

---

## 📊 Dashboard Features (All Dynamic)

### Stats Cards
```typescript
// Fetched from API every 30 seconds
- Total Queries: Real count from database
- Knowledge Gaps: Live gap detection
- Avg Confidence: Calculated from responses
- Active Users: Tracked sessions
```

### Knowledge Gaps Table
```typescript
// Real-time data
- Question: From user queries
- Count: Occurrence tracking
- Confidence: AI confidence score
- Status: New/Reviewing/Resolved
- Last Asked: Timestamp
- Actions: Resolve button
```

### Search & Filter
- Live search across all questions
- Instant filtering
- No page reload

---

## 🔧 API Endpoints Used

### Chat
```
GET /api/MutualFund/ask?query={query}&userId={userId}
```

### Dashboard
```
GET /api/KnowledgeGap/dashboard
POST /api/KnowledgeGap/sync-to-qdrant
```

---

## 🎯 Tech Stack

### Backend
- ASP.NET Core 9.0
- SQL Server
- Qdrant Vector DB
- Ollama LLM

### Frontend
- React 19.2.4
- TypeScript 5.9.3
- Tailwind CSS 4.2.2
- Vite 8.0.1

---

## 📱 Views

### 1. Chat View
- Clean chat interface
- User messages (right, green)
- AI messages (left, white)
- Source badges
- Confidence scores

### 2. Dashboard View
- Stats overview
- Knowledge gaps table
- Search functionality
- Sync button

---

## 🎨 Design System

### Colors
```css
Primary: #0f766e (Emerald)
Primary Light: #14b8a6
Primary Dark: #0d5c54
Background: #f8fafc (Slate 50)
```

### Typography
```css
Font: Inter
Headings: Bold, 700
Body: Regular, 400
Small: 300
```

### Components
- Rounded corners: 12-16px
- Shadows: Subtle, layered
- Animations: Smooth, 300ms
- Hover effects: Scale & shadow

---

## 🔄 Data Flow

```
User Query → Frontend → Backend API → LLM/Database
                                    ↓
                              Response
                                    ↓
                         Frontend Updates UI
```

### Dashboard Data Flow
```
Dashboard Component → API Call (every 30s)
                           ↓
                    Knowledge Gap API
                           ↓
                    Stats + Gaps Data
                           ↓
                    Update UI (Dynamic)
```

---

## 🐛 Troubleshooting

### Backend won't start?
```bash
# Check port
netstat -ano | findstr :44328

# Kill process
taskkill /PID <pid> /F
```

### Frontend won't start?
```bash
# Check port
netstat -ano | findstr :3000

# Change port in vite.config.ts
server: { port: 3001 }
```

### API connection error?
- Ensure backend is running
- Check CORS settings
- Verify API URL in App.tsx

---

## 📁 Project Structure

```
d:\AI_Fund\Ai_Fund\
├── Ai_Fund\                    # Backend
│   ├── Controllers\
│   ├── Services\
│   ├── Models\
│   └── Program.cs
├── frontend\                   # Frontend
│   ├── src\
│   │   ├── components\
│   │   │   ├── Header.tsx
│   │   │   ├── Dashboard.tsx
│   │   │   ├── ChatMessage.tsx
│   │   │   ├── ChatInput.tsx
│   │   │   └── ...
│   │   ├── types\
│   │   ├── App.tsx
│   │   └── main.tsx
│   ├── index.html
│   └── package.json
└── START-FULLSTACK.bat         # One-click startup
```

---

## ✅ Checklist

Before starting:
- [ ] .NET 9.0 SDK installed
- [ ] Node.js installed
- [ ] SQL Server running
- [ ] Ollama running
- [ ] Dependencies installed (`npm install`)

After starting:
- [ ] Backend responds at https://localhost:44328
- [ ] Frontend loads at http://localhost:3000
- [ ] Chat works
- [ ] Dashboard shows data
- [ ] Stats update dynamically

---

## 🚀 Production Build

### Frontend
```bash
cd frontend
npm run build
```

Output: `dist/` folder

### Backend
```bash
cd Ai_Fund
dotnet publish -c Release
```

---

## 📈 Performance

- **Frontend**: Vite for fast HMR
- **Backend**: .NET 9.0 optimized
- **Database**: Indexed queries
- **Vector DB**: Qdrant for fast search
- **Auto-refresh**: 30s interval (configurable)

---

## 🎯 Key Features Summary

✅ **100% Dynamic** - No static data
✅ **Real-time Updates** - Auto-refresh every 30s
✅ **Professional UI** - Clean, modern design
✅ **Responsive** - Mobile & desktop
✅ **Type-safe** - TypeScript throughout
✅ **Component-based** - Reusable code
✅ **Production-ready** - Optimized builds

---

**Made with ❤️ by Mehak + Amazon Q**
