export type JwtPayload = Record<string, unknown>;

function base64UrlDecode(input: string): string {
  const base64 = input.replace(/-/g, '+').replace(/_/g, '/');
  const pad = base64.length % 4 === 0 ? '' : '='.repeat(4 - (base64.length % 4));
  return atob(base64 + pad);
}

export function decodeJwt(token: string): JwtPayload | null {
  try {
    const parts = token.split('.');
    if (parts.length !== 3) return null;
    const json = base64UrlDecode(parts[1]);
    return JSON.parse(json) as JwtPayload;
  } catch {
    return null;
  }
}

export function getRolesFromToken(token: string): string[] {
  const payload = decodeJwt(token);
  if (!payload) return [];

  // ASP.NET uses ClaimTypes.Role -> often emitted as this URI in JWT payload
  const roleKey = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
  const value = (payload[roleKey] ?? payload['role']) as unknown;

  if (Array.isArray(value)) return value.filter(v => typeof v === 'string') as string[];
  if (typeof value === 'string') return [value];
  return [];
}

