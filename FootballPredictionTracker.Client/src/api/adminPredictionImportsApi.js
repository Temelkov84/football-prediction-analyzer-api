export async function importPredictionsFromCsv(file) {
  const formData = new FormData()
  formData.append('file', file)

  const response = await fetch('/api/admin/prediction-imports/csv', {
    method: 'POST',
    body: formData,
  })

  const contentType = response.headers.get('content-type')
  const data = contentType?.includes('application/json')
    ? await response.json()
    : await response.text()

  if (!response.ok) {
    throw new Error(formatApiError(data))
  }

  return data
}

function formatApiError(data) {
  if (!data) {
    return 'Failed to import predictions.'
  }

  if (typeof data === 'string') {
    return data
  }

  if (data.message) {
    return data.message
  }

  if (Array.isArray(data.errors)) {
    return data.errors.join('\n')
  }

  if (data.errors && typeof data.errors === 'object') {
    return Object.entries(data.errors)
      .map(([field, value]) => {
        if (Array.isArray(value)) {
          return `${field}: ${value.join(', ')}`
        }

        if (typeof value === 'string') {
          return `${field}: ${value}`
        }

        return `${field}: ${JSON.stringify(value)}`
      })
      .join('\n')
  }

  return JSON.stringify(data, null, 2)
}