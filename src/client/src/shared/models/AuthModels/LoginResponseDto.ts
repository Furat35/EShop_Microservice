export class LoginResponseDto {
  constructor(init?: Partial<LoginResponseDto>) {
    Object.assign(this, init)
  }
  accessToken: string
  refreshToken: string
  userName: string
}
