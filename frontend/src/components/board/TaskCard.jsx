import { useState } from 'react'
import TaskModal from '../modals/TaskModal.jsx'

export default function TaskCard({ columnId, task, onEdit, onDelete }) {
  const [open, setOpen] = useState(false)

  return (
    <>
      <div className="rounded-lg border p-3 bg-white">
        <div className="flex items-start justify-between">
          <div>
            <h4 className="font-medium">{task.title}</h4>
            {task.desc && <p className="mt-1 text-sm text-gray-600">{task.desc}</p>}
          </div>
          <div className="flex gap-2">
            <button
              onClick={() => setOpen(true)}
              className="rounded-md border px-2 py-1 text-sm hover:bg-gray-50"
            >
              Edit
            </button>
            <button
              onClick={onDelete}
              className="rounded-md border px-2 py-1 text-sm text-red-600 hover:bg-red-50"
            >
              Delete
            </button>
          </div>
        </div>
        {task.dueDate && (
          <span className="mt-2 inline-block rounded bg-gray-100 px-2 py-1 text-xs text-gray-700">
            Due: {task.dueDate}
          </span>
        )}
      </div>

      {open && (
        <TaskModal
          initial={task}
          onClose={() => setOpen(false)}
          onSubmit={(data) => {
            onEdit(data)
            setOpen(false)
          }}
        />
      )}
    </>
  )
}
