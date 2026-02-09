export type AppUser = {
    id: string;
    displayName: string;
    email: string;
    password: string;
    gender: string;
    address: string;
    dateOfBirth: string;
    phoneNumber: string;
}

export type LoginUser = {
    email: string;
    password: string;
}

export type UserDto = {
    id: string;
    email: string;
    displayName: string;
    token: string;
}