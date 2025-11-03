const BASE_URL = import.meta.env.VITE_API_URL;

async function sendHttpRequest(endpointPath, { method = 'GET', body } = {}) {
    const response = await fetch(`${BASE_URL}${endpointPath}`,{
        method,
        headers: body ? {'Content-Type' : 'application/json'} : undefined,
        body: body ? JSON.stringify(body) : undefined,
    });

    if(!response.ok){
        let errorPayload;
        try {
            errorPayload = await response.json();
        }
        catch{
            errorPayload = {title:  response.statusText};
        }
        const error = new Error(errorPayload.title || 'Request failed');
        error.status = response.status;
        error.details = errorPayload;
        throw error;
    }
    if (response.status === 204) return null;
    return response.json();
}

export const api = {

    // For Column

    getBoard: () => sendHttpRequest('/api/board'),
    createColumn: (title, id) => sendHttpRequest('/api/columns', {method: 'POST', body: {title, id}}),
    renameColumn: (id, title) => sendHttpRequest(`/api/columns/${id}`, {method: 'PATCH', body: {title}}),
    deleteColumn: (id) => sendHttpRequest(`/api/columns/${id}`, {method: 'DELETE'}),
    
    // For Task

    createTask: (columnId, task) => sendHttpRequest(`/api/columns/${columnId}/tasks`, {method: 'POST', body: task}),
    updateTask: (taskId, patch) => sendHttpRequest(`/api/tasks/${taskId}`, {method: 'PATCH', body: patch}),
    deleteTask: (taskId) => sendHttpRequest(`/api/tasks/${taskId}`, {method: 'DELETE'}),
};