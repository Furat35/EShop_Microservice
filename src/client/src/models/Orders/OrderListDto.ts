import type { OrderItemListDto } from '../OrderItems/OrderItemListDto'

export class OrderListDto {
  id: string = ''
  createDate: number = 0
  status: number = 0
  description: string = ''
  street: string = ''
  city: string = ''
  zipcode: string = ''
  country: string = ''
  orderItems: OrderItemListDto[] = []
  total: number = 0
}
