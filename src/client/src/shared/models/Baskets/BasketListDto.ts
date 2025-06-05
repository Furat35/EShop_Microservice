import type { BasketItemListDto } from '../BasketItems/BasketItemListDto'

export class BasketListDto {
  userId: number = 0
  items: BasketItemListDto[] = []
}
