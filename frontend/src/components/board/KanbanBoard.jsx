import { useState } from 'react'
import { Toaster, toast } from 'react-hot-toast'
import useKanbanData from '../../hooks/useKanbanData.js'
import Column from './Column.jsx'
import ColumnModal from '../modals/ColumnModal.jsx'

export default function KanbanBoard() {
  const {
    columns,
    addColumn,
    updateColumnTitle,
    deleteColumn,
    addTask,
    updateTask,
    deleteTask,
  } = useKanbanData()

  const [addColumnOpen, setAddColumnOpen] = useState(false)

  return (
    <div className="min-h-screen bg-gradient-to-b from-slate-50 to-white relative py-10">
      {/* <div className="absolute inset-0 overflow-hidden"> */}
        <div className="min-wh-screen absolute -top-28 -left-12 h-80 w-80 rounded-full bg-blue-100 blur-3xl" />
        <div className="absolute top-36 -right-12 h-80 w-80 rounded-full bg-indigo-100 blur-3xl" />
      {/* </div> */}

      <Toaster position="top-right" />

      {/* Header */}
      <header className="sticky top-4 z-40 w-full flex justify-center">
        <div className="mx-auto max-w-7xl w-full px-4 sm:px-6">
          <div className="flex h-16 items-center justify-between rounded-2xl bg-white/80 border border-slate-200/70 px-4 py-2 shadow-md backdrop-blur">
            <div className="flex items-center gap-3">
              <h1 className="text-xl font-semibold tracking-tight text-slate-900">
                Kanban Board
              </h1>
            </div>

            <button
              onClick={() => setAddColumnOpen(true)}
              className = "text-white bg-blue-700 hover:bg-blue-800 focus:outline-none focus:ring-4 focus:ring-blue-300 font-medium rounded-full text-sm px-5 py-2.5 text-center me-2 mb-2 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800 hover:cursor-pointer"
            >
             + Add Column
            </button>
          </div>
        </div>
      </header>

      {/* Content */}
      <main className="mx-auto max-w-7xl px-4 sm:px-6 py-8">
        {columns.length === 0 ? (
          <div className="mx-auto w-full rounded-2xl border border-dashed border-slate-300 bg-white/70 p-12 text-center text-slate-600 backdrop-blur-sm shadow-lg">
            <p className="text-sm">
              Your board is empty. Click <span className="font-medium text-slate-800">Add Column</span> to start.
            </p>
          </div>
        ) : (
          <div
            className="flex gap-6 overflow-x-auto pb-6 pr-2 snap-x snap-mandatory scrollbar-thin scrollbar-thumb-slate-300/60"
          >
            {columns.map((col) => (
              <div key={col.id} className="snap-start min-w-[18rem]">
                <div className="rounded-xl bg-white/90 border border-slate-100 shadow-lg p-3 transition-shadow hover:shadow-xl">
                  <Column
                    column={col}
                    onAddTask={(columnId, data) => {
                      addTask(columnId, data)
                      toast.success('Task added')
                    }}
                    onEditTask={(columnId, taskId, patch) => {
                      updateTask(columnId, taskId, patch)
                      toast.success('Task updated')
                    }}
                    onDeleteTask={(columnId, taskId) => {
                      deleteTask(columnId, taskId)
                      toast.success('Task deleted')
                    }}
                    onEditColumnTitle={(columnId, title) => {
                      updateColumnTitle(columnId, title)
                      toast.success('Column title updated')
                    }}
                    onDeleteColumn={(columnId) => {
                      deleteColumn(columnId)
                      toast.success('Column deleted')
                    }}
                  />
                </div>
              </div>
            ))}
          </div>
        )}
      </main>

      {addColumnOpen && (
        <ColumnModal
          onClose={() => setAddColumnOpen(false)}
          onSubmit={(title) => {
            addColumn(title)
            setAddColumnOpen(false)
            toast.success('Column added')
          }}
        />
      )}
    </div>
  )
}
