import { useEffect, useState } from 'react'

export default function ColumnModal({ initialTitle = '', onClose, onSubmit }) {
  const [title, setTitle] = useState(initialTitle)
  const [err, setErr] = useState('')

  useEffect(() => {
    const onKey = (e) => e.key === 'Escape' && onClose()
    window.addEventListener('keydown', onKey)
    return () => window.removeEventListener('keydown', onKey)
  }, [onClose])

  const submit = (e) => {
    e.preventDefault()
    if (!title.trim()) return setErr('Title is required')
    onSubmit(title.trim())
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
              {initialTitle ? 'Edit Column' : 'Add Column'}
            </h3>
            <button type="button" onClick={onClose} aria-label="Close">
              ✕
            </button>
          </div>
          <label className="mb-1 block text-sm font-medium">Column title</label>
          <input
            autoFocus
            value={title}
            onChange={(e) => {
              setTitle(e.target.value)
              setErr('')
            }}
            className="w-full rounded-md border px-3 py-2 text-sm focus:ring-2 focus:ring-blue-500"
            placeholder='e.g. "To Do"'
          />
          {err && <p className="mt-1 text-sm text-red-600">{err}</p>}

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
