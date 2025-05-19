import { CatalogBrandListDto } from '../CatalogBrands/CatalogBrandListDto'
import { CatalogTypeListDto } from '../CatalogTypes/CatalogTypeListDto'

export class CatalogItemListDto {
  id: number = 0
  name: string = ''
  description: string = ''
  price: number = 0
  pictureFileName: string = ''
  pictureUri: string = ''
  availableStock: number = 0
  onReorder: boolean = false
  catalogType: CatalogTypeListDto = new CatalogTypeListDto()
  catalogBrand: CatalogBrandListDto = new CatalogBrandListDto()
}
