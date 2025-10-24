import { useEffect, useState } from 'react'

export default function TaskModal({ initial = null, onClose, onSubmit }) {
  const [title, setTitle] = useState(initial?.title || '')
  const [desc, setDesc] = useState(initial?.desc || '')
  const [dueDate, setDueDate] = useState(initial?.dueDate || '')
  const [errors, setErrors] = useState({})

  useEffect(() => {
    const onKey = (e) => e.key === 'Escape' && onClose()
    window.addEventListener('keydown', onKey)
    return () => window.removeEventListener('keydown', onKey)
  }, [onClose])

  const validate = () => {
    const e = {}
    if (!title.trim()) e.title = 'Title is required'
    if (title.length > 100) e.title = 'Max 100 chars'
    if (desc.length > 1000) e.desc = 'Max 1000 chars'
    return e
  }

  const submit = (e) => {
    e.preventDefault()
    const v = validate()
    if (Object.keys(v).length) return setErrors(v)
    onSubmit({ title: title.trim(), desc: desc.trim(), dueDate })
  }

  return (
    <div className="fixed inset-0 z-50">
      <div className="absolute inset-0 bg-black/30" onClick={onClose} />
      <div className="absolute inset-0 flex items-center justify-center p-4">
        <form
          onSubmit={submit}
          className="w-full max-w-md rounded-xl bg-white p-5 shadow-lg"
          role="dialog"
          aria-modal="true"
        >
          <div className="mb-4 flex items-center justify-between">
            <h3 className="text-lg font-semibold">
              {initial ? 'Edit Task' : 'Add Task'}
            </h3>
            <button type="button" onClick={onClose} aria-label="Close" >
              ✕
            </button>
          </div>

          <div className="space-y-4">
            <div>
              <label className="mb-1 block text-sm font-medium">Title</label>
              <input
                value={title}
                onChange={(e) => setTitle(e.target.value)}
                className="w-full rounded-md border px-3 py-2 text-sm focus:ring-2 focus:ring-blue-500"
                placeholder="Add task(s)"
              />
              {errors.title && (
                <p className="mt-1 text-sm text-red-600">{errors.title}</p>
              )}
            </div>
            <div>
              <label className="mb-1 block text-sm font-medium">Description</label>
              <textarea
                value={desc}
                onChange={(e) => setDesc(e.target.value)}
                className="h-24 w-full resize-none rounded-md border px-3 py-2 text-sm focus:ring-2 focus:ring-blue-500"
                placeholder="Optional details…"
              />
              {errors.desc && (
                <p className="mt-1 text-sm text-red-600">{errors.desc}</p>
              )}
            </div>
            <div>
              <label className="mb-1 block text-sm font-medium">Due Date</label>
              <input
                type="date"
                value={dueDate}
                onChange={(e) => setDueDate(e.target.value)}
                className="w-full rounded-md border px-3 py-2 text-sm focus:ring-2 focus:ring-blue-500"
              />
            </div>
          </div>

          <div className="mt-6 flex justify-end gap-2">
            <button
              type="button"
              onClick={onClose}
              className="rounded-md border px-4 py-2 text-sm hover:bg-gray-50"
            >
              Cancel
            </button>
            <button
              type="submit"
              className="rounded-md bg-blue-600 px-4 py-2 text-sm text-white hover:bg-blue-700"
            >
              Save
            </button>
          </div>
        </form>
      </div>
    </div>
  )
}
