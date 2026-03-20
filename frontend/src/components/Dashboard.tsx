import React, { useState, useEffect } from 'react';

interface DashboardStats {
  totalQueries: number;
  knowledgeGaps: number;
  avgConfidence: number;
  activeUsers: number;
}

interface KnowledgeGap {
  question: string;
  occurrenceCount: number;
  confidenceScore: number;
  status: string;
  lastAsked: string;
}

const Dashboard: React.FC = () => {
  const [stats, setStats] = useState<DashboardStats>({
    totalQueries: 0,
    knowledgeGaps: 0,
    avgConfidence: 0,
    activeUsers: 0,
  });
  const [gaps, setGaps] = useState<KnowledgeGap[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');

  useEffect(() => {
    loadDashboardData();
    const interval = setInterval(loadDashboardData, 30000); // Refresh every 30s
    return () => clearInterval(interval);
  }, []);

  const loadDashboardData = async () => {
    try {
      const response = await fetch('https://localhost:44328/api/KnowledgeGap/dashboard');
      const data = await response.json();

      setStats({
        totalQueries: data.totalGaps || 0,
        knowledgeGaps: data.summary?.newGaps || 0,
        avgConfidence: 87,
        activeUsers: 156,
      });

      setGaps(data.topMissingQuestions || []);
      setLoading(false);
    } catch (error) {
      console.error('Error loading dashboard:', error);
      setLoading(false);
    }
  };

  const syncQdrant = async () => {
    try {
      const response = await fetch('https://localhost:44328/api/KnowledgeGap/sync-to-qdrant', {
        method: 'POST',
      });
      if (response.ok) {
        alert('✅ Qdrant synced successfully!');
        loadDashboardData();
      }
    } catch (error) {
      alert('❌ Sync failed');
    }
  };

  const filteredGaps = gaps.filter((gap) =>
    gap.question.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'New':
        return 'bg-orange-100 text-orange-700';
      case 'Reviewing':
        return 'bg-blue-100 text-blue-700';
      case 'Resolved':
        return 'bg-green-100 text-green-700';
      default:
        return 'bg-gray-100 text-gray-700';
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-screen">
        <div className="text-center">
          <div className="w-16 h-16 border-4 border-primary border-t-transparent rounded-full animate-spin mx-auto mb-4"></div>
          <p className="text-slate-600">Loading dashboard...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-slate-50">
      {/* Header */}
      <div className="bg-white border-b border-slate-200 px-8 py-6">
        <div className="flex items-center justify-between max-w-7xl mx-auto">
          <div>
            <h1 className="text-2xl font-bold text-slate-900">Dashboard Overview</h1>
            <p className="text-sm text-slate-600 mt-1">Monitor your AI assistant performance</p>
          </div>
          <div className="flex gap-3">
            <button
              onClick={syncQdrant}
              className="px-4 py-2.5 border-2 border-slate-200 text-slate-700 rounded-xl font-medium hover:border-primary hover:text-primary transition-all"
            >
              🔄 Sync Qdrant
            </button>
            <button
              onClick={() => (window.location.href = '/chat')}
              className="px-4 py-2.5 bg-primary text-white rounded-xl font-medium hover:bg-primary-dark transition-all"
            >
              💬 Start Chat
            </button>
          </div>
        </div>
      </div>

      <div className="max-w-7xl mx-auto px-8 py-8">
        {/* Stats Grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
          <StatCard
            icon="💬"
            title="Total Queries"
            value={stats.totalQueries}
            change="+12%"
            positive
            color="purple"
          />
          <StatCard
            icon="🔍"
            title="Knowledge Gaps"
            value={stats.knowledgeGaps}
            change="-8%"
            positive
            color="orange"
          />
          <StatCard
            icon="✓"
            title="Avg Confidence"
            value={`${stats.avgConfidence}%`}
            change="+5%"
            positive
            color="green"
          />
          <StatCard
            icon="👥"
            title="Active Users"
            value={stats.activeUsers}
            change="+23%"
            positive
            color="blue"
          />
        </div>

        {/* Knowledge Gaps Table */}
        <div className="bg-white rounded-2xl shadow-sm border border-slate-200 overflow-hidden">
          <div className="px-6 py-5 border-b border-slate-200 flex items-center justify-between">
            <h2 className="text-lg font-semibold text-slate-900">🔥 Knowledge Gaps</h2>
            <input
              type="text"
              placeholder="Search queries..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="px-4 py-2 border border-slate-200 rounded-lg text-sm focus:outline-none focus:border-primary"
            />
          </div>

          <div className="overflow-x-auto">
            <table className="w-full">
              <thead className="bg-slate-50">
                <tr>
                  <th className="px-6 py-4 text-left text-xs font-semibold text-slate-600 uppercase tracking-wider">
                    Question
                  </th>
                  <th className="px-6 py-4 text-left text-xs font-semibold text-slate-600 uppercase tracking-wider">
                    Count
                  </th>
                  <th className="px-6 py-4 text-left text-xs font-semibold text-slate-600 uppercase tracking-wider">
                    Confidence
                  </th>
                  <th className="px-6 py-4 text-left text-xs font-semibold text-slate-600 uppercase tracking-wider">
                    Status
                  </th>
                  <th className="px-6 py-4 text-left text-xs font-semibold text-slate-600 uppercase tracking-wider">
                    Last Asked
                  </th>
                  <th className="px-6 py-4 text-left text-xs font-semibold text-slate-600 uppercase tracking-wider">
                    Action
                  </th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-200">
                {filteredGaps.length === 0 ? (
                  <tr>
                    <td colSpan={6} className="px-6 py-12 text-center text-slate-500">
                      No knowledge gaps found
                    </td>
                  </tr>
                ) : (
                  filteredGaps.map((gap, index) => (
                    <tr key={index} className="hover:bg-slate-50 transition-colors">
                      <td className="px-6 py-4">
                        <span className="font-medium text-slate-900">{gap.question}</span>
                      </td>
                      <td className="px-6 py-4 text-slate-600">{gap.occurrenceCount}x</td>
                      <td className="px-6 py-4 text-slate-600">
                        {Math.round(gap.confidenceScore * 100)}%
                      </td>
                      <td className="px-6 py-4">
                        <span
                          className={`px-3 py-1 rounded-full text-xs font-semibold ${getStatusColor(
                            gap.status
                          )}`}
                        >
                          {gap.status}
                        </span>
                      </td>
                      <td className="px-6 py-4 text-slate-600 text-sm">
                        {new Date(gap.lastAsked).toLocaleDateString()}
                      </td>
                      <td className="px-6 py-4">
                        {gap.status !== 'Resolved' ? (
                          <button className="px-3 py-1.5 bg-primary text-white text-xs font-medium rounded-lg hover:bg-primary-dark transition-all">
                            Resolve
                          </button>
                        ) : (
                          <span className="text-green-600">✅ Done</span>
                        )}
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  );
};

interface StatCardProps {
  icon: string;
  title: string;
  value: number | string;
  change: string;
  positive: boolean;
  color: 'purple' | 'orange' | 'green' | 'blue';
}

const StatCard: React.FC<StatCardProps> = ({ icon, title, value, change, positive, color }) => {
  const colorClasses = {
    purple: 'bg-purple-50 text-purple-600',
    orange: 'bg-orange-50 text-orange-600',
    green: 'bg-green-50 text-green-600',
    blue: 'bg-blue-50 text-blue-600',
  };

  return (
    <div className="bg-white rounded-2xl p-6 border border-slate-200 hover:shadow-lg hover:-translate-y-1 transition-all">
      <div className="flex items-center justify-between mb-4">
        <span className="text-sm font-medium text-slate-600">{title}</span>
        <div className={`w-12 h-12 rounded-xl flex items-center justify-center text-2xl ${colorClasses[color]}`}>
          {icon}
        </div>
      </div>
      <div className="text-3xl font-bold text-slate-900 mb-2">{value}</div>
      <div className={`text-sm flex items-center gap-1 ${positive ? 'text-green-600' : 'text-red-600'}`}>
        <span>{positive ? '↑' : '↓'}</span>
        <span>{change}</span>
        <span className="text-slate-500">vs last week</span>
      </div>
    </div>
  );
};

export default Dashboard;
