import { useState, useCallback } from 'react'

export default function useKanbanData() {
  const [columns, setColumns] = useState([]) // [{id,title,tasks:[{id,title,desc,dueDate}]}]

  const addColumn = useCallback((title) => {
    const id = crypto.randomUUID()
    setColumns((prev) => [...prev, { id, title: title.trim(), tasks: [] }])
    return id
  }, [])

  const updateColumnTitle = useCallback((columnId, title) => {
    setColumns((prev) =>
      prev.map((c) => (c.id === columnId ? { ...c, title: title.trim() } : c)),
    )
  }, [])

  const deleteColumn = useCallback((columnId) => {
    setColumns((prev) => prev.filter((c) => c.id !== columnId))
  }, [])

  const addTask = useCallback((columnId, task) => {
    const id = crypto.randomUUID()
    setColumns((prev) =>
      prev.map((c) =>
        c.id === columnId ? { ...c, tasks: [{ id, ...task }, ...c.tasks] } : c,
      ),
    )
  }, [])

  const updateTask = useCallback((columnId, taskId, patch) => {
    setColumns((prev) =>
      prev.map((c) =>
        c.id !== columnId
          ? c
          : {
              ...c,
              tasks: c.tasks.map((t) => (t.id === taskId ? { ...t, ...patch } : t)),
            },
      ),
    )
  }, [])

  const deleteTask = useCallback((columnId, taskId) => {
    setColumns((prev) =>
      prev.map((c) =>
        c.id !== columnId ? c : { ...c, tasks: c.tasks.filter((t) => t.id !== taskId) },
      ),
    )
  }, [])

  return {
    columns,
    addColumn,
    updateColumnTitle,
    deleteColumn,
    addTask,
    updateTask,
    deleteTask,
  }
}
