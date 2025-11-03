import { useState, useCallback, useEffect } from 'react';
import { api } from '../lib/api';

export default function useKanbanData() {
  const [columns, setColumns] = useState([]); // [{id,title,tasks:[{id,title,desc,dueDate}]}]
  const [loading, setLoading] = useState(true); 
  const [error, setError] = useState(null); 

  const load = useCallback(async () => {
    try{
      setError(null);
      const kanbanBoardResponse = await api.getBoard();
      const normalizedColumns = (kanbanBoardResponse.columns ?? []).map(column => ({ ...column, tasks: column.tasks ?? [] }));
      setColumns(normalizedColumns);
    }
    catch(error){
      setError(error);
    }
    finally{
      setLoading(false);
    }
  }, []);

  useEffect(() => {load(); }, [load]);

  // Columns
  const addColumn = useCallback(async (title) => {
    await api.createColumn(title);
    await load();
  }, [load]);

  const updateColumnTitle = useCallback(async (columnId, title) => {
    await api.renameColumn(columnId, title);
    await load();
  }, [load]);

  const deleteColumn = useCallback(async(columnId) => {
    await api.deleteColumn(columnId);
    await load();
  }, [load]);

  // Tasks
  const addTask = useCallback(async (columnId, task) => {
    await api.createTask(columnId, task);
    await load();
  }, [load]);

  const updateTask = useCallback(async (_columnId, taskId, patch) => {
    await api.updateTask(taskId, patch);
    await load();
  }, [load]);

  const deleteTask = useCallback(async (_columnId, taskId) => {
    await api.deleteTask(taskId);
    await load();
  }, [load]);

  return {
    columns,
    loading,
    error,
    addColumn,
    updateColumnTitle,
    deleteColumn,
    addTask,
    updateTask,
    deleteTask,
  };
}
