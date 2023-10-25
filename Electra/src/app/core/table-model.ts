export interface PaginatedListParams {
  start?: number;
  pageSize?: number;
  orderBy?: { name: string; dir: 'asc' | 'desc' }[];
  search?: string;
  filter?: string;
  top?: number;
  select?: string[];
  expand?: string[];
}

export interface PaginatedListResults<T> {
  start: number;
  length: number;
  total: number;
  data: T[];
}

export const defaultPageIndex = 0;
export const defaultPageSize = 10;
