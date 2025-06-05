import type { OrderItemUpdateDto } from '../OrderItems/OrderItemUpdateDto'

export class OrderUpdateDto {
  id: string = ''
  orderNumber: string = ''
  date: number = 0
  status: number = 0
  description: string = ''
  street: string = ''
  city: string = ''
  zipcode: string = ''
  country: string = ''
  orderItems: OrderItemUpdateDto[] = []
  total: number = 0
}
