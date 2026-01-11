export interface Song {
  id: number | string
  title: string
  artist?: string
  year?: number
  genre?: string
  fileUrl?: string
}

export interface Album {
  id: number | string
  name: string
  artist?: string
  releaseYear?: number
  createdBy?: string
  songs?: Song[]
}

export interface Playlist {
  id: number | string
  name: string
  description?: string
  createdBy?: string
  songs?: Song[]
}