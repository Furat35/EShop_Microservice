export class BasketCheckoutDto {
  city: string = ''
  street: string = ''
  state: string = ''
  country: string = ''
  zipCode: string = ''
  cardNumber: string = ''
  cardHolderName: string = ''
  cardExpiration: Date = new Date()
  cardSecurityNumber: string = ''
  cardTypeId: number = 0
  description: string = ''
}
