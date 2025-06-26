export class PaginationModel<T> {
  page: number = 0
  pageSize: number = 0
  count: number = 0
  // totalPages: number = 0
  hasNext: boolean = false
  hasPrevious: boolean = false
  data: T[] = []
}
