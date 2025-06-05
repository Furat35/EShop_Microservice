export class CatalogItemCreateDto {
  id: number = 0
  name: string = ''
  description: string = ''
  price: number = 0
  pictureFileName: string = ''
  pictureUri: string = ''
  availableStock: number = 0
  onReorder: boolean = false
  catalogTypeId: number = 0
  catalogBrandId: number = 0
}
