import { useState, useCallback, useEffect } from 'react';
import { api } from '../lib/api';

export default function useKanbanData() {
  const [columns, setColumns] = useState([]);
  const [loading, setLoading] = useState(false); 
  const [error, setError] = useState(null); 

  const fetchBoardFromApi = useCallback (async()=> {
    setLoading(true);
    try{
      setError(null);

      const kanbanBoardResponse = await api.getBoard();
      const normalizedColumns = (kanbanBoardResponse.columns ?? []).map(column => ({ ...column, tasks: column.tasks ?? [], }));

      setColumns(normalizedColumns);
    } catch (err) {
      setError(err)
    } finally{
      setLoading(false);
    }
  }, []);

  // Fetch once on mount
  useEffect(() => {
  fetchBoardFromApi();
}, [fetchBoardFromApi]);


  // Columns
  const addColumn = useCallback(async (title) => {
    setLoading(true);
    try {
      await api.createColumn(title);
      await fetchBoardFromApi();
    } finally{
      setLoading(false);
    }
  }, [fetchBoardFromApi]);

  const updateColumnTitle = useCallback(async (columnId, title) => {
    setLoading(true);
    try{
      await api.renameColumn(columnId, title);
      await fetchBoardFromApi();
    } finally {
      setLoading(false);
    }
  }, [fetchBoardFromApi]);

  const deleteColumn = useCallback(async(columnId) => {
    setLoading(true);
    try{
      await api.deleteColumn(columnId);
      await fetchBoardFromApi();
    } finally {
      setLoading(false);
    }
  }, [fetchBoardFromApi]);

  // Tasks
  const addTask = useCallback(async (columnId, task) => {
    setLoading(true);
    try {
      await api.createTask(columnId, task);
      await fetchBoardFromApi();
    } finally{
      setLoading(false);
    }  
  }, [fetchBoardFromApi]);

  const updateTask = useCallback(async (_columnId, taskId, patch) => {
    setLoading(true);
    try {
      await api.updateTask(taskId, patch);
      await fetchBoardFromApi();
    } finally{
      setLoading(false);
    }  
  }, [fetchBoardFromApi]);
  
  const deleteTask = useCallback(async (_columnId, taskId) => {
    setLoading(true);
    try {
      await api.deleteTask(taskId);
      await fetchBoardFromApi();
    } finally {
      setLoading(false);
    }  
  }, [fetchBoardFromApi]);

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
