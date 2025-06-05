import type { OrderItemCreateDto } from '../OrderItems/OrderItemCreateDto'

export class OrderCreateDto {
  orderNumber: string = ''
  date: number = 0
  status: number = 0
  description: string = ''
  street: string = ''
  city: string = ''
  zipcode: string = ''
  country: string = ''
  orderItems: OrderItemCreateDto[] = []
  total: number = 0
}
