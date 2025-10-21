export default function App() {
    return (
        <div className="min-h-screen bg-black text-white">
            <header className="border-b border-zinc-800 px-6 py-4">
                <h1 className="text-2xl font-bold">Kanban App</h1>
            </header>

            <main className="p-6">
                <div className="max-w-md rounded-lg border border-zinc-800 bg-zinc-900/60 p-5">
                    <h2 className="text-lg font-semibold mb-2">Tailwind check</h2>
                    <p className="text-sm text-zinc-300 mb-4">
                        If this box has rounded corners, a subtle border, and spacing, Tailwind is working.
                    </p>
                    <button className="rounded-md bg-indigo-600 px-4 py-2 text-sm font-medium hover:bg-indigo-500 focus:outline-none focus:ring-2 focus:ring-indigo-400">
                        Test Button
                    </button>
                </div>
            </main>
        </div>
    )
}
