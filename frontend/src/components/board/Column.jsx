import { useState } from 'react'
import TaskCard from './TaskCard'
import TaskModal from '../modals/TaskModal'
import ColumnModal from '../modals/ColumnModal'

export default function Column({
  column,
  onAddTask,
  onEditTask,
  onDeleteTask,
  onEditColumnTitle,
  onDeleteColumn,
}) {
  const [taskOpen, setTaskOpen] = useState(false)
  const [titleOpen, setTitleOpen] = useState(false)

  return (
    <div className="rounded-xl border bg-blue shadow-sm">
      <div className="flex items-center justify-between border-b px-4 py-3">
        <h3 className="font-semibold">{column.title}</h3>
        <div className="flex items-center gap-2">
          <button
            onClick={() => setTitleOpen(true)}
            className="rounded-md border px-3 py-1.5 text-sm hover:bg-gray-50 hover:cursor-pointer"
          >
            Edit
          </button>
          <button
            onClick={() => {
                if (window.confirm('Are you sure you want to delete this column ?')) {
                    onDeleteColumn(column.id)}
            }}    
            className="rounded-md border px-3 py-1.5 text-sm text-red-600 hover:bg-red-50 hover:cursor-pointer"
          >
            Delete
          </button>
        </div>
      </div>

      <div className="px-4 py-4">
        <button
          onClick={() => setTaskOpen(true)}
          className="mb-3 w-full rounded-md border border-dashed px-3 py-2 text-left text-sm hover:bg-gray-50 hover:cursor-pointer"
        >
          + Add Task
        </button>

        {column.tasks.length === 0 ? (
          <p className="select-none rounded-md border bg-gray-50 px-3 py-6 text-center text-sm text-gray-900">
            No tasks yet. Click “Add Task”.
          </p>
        ) : (
          <div className="space-y-3">
            {column.tasks.map((t) => (
              <TaskCard
                key={t.id}
                columnId={column.id}
                task={t}
                onEdit={(patch) => onEditTask(column.id, t.id, patch)}
                onDelete={() => {
                    if (window.confirm('Are you sure you want to delete this task?')) {
                        onDeleteTask(column.id, t.id)
                }
                }}
              />
            ))}
          </div>
        )}
      </div>

      {taskOpen && (
        <TaskModal
          onClose={() => setTaskOpen(false)}
          onSubmit={(data) => {
            onAddTask(column.id, data)
            setTaskOpen(false)
          }}
        />
      )}

      {titleOpen && (
        <ColumnModal
          initialTitle={column.title}
          onClose={() => setTitleOpen(false)}
          onSubmit={(title) => {
            onEditColumnTitle(column.id, title)
            setTitleOpen(false)
          }}
        />
      )}
    </div>
  )
}
