const appHost = 'https://webportal.mellongroup.com/MELLON_INTERNAL_APPS/approvals';
const apiHost = 'https://webportal.mellongroup.com/MELLON_INTERNAL_APPS';

export const environment = {
  production: true,
  serviceApprovalsUrl: `${apiHost}/api/approvals`,
  auth: {
    clientId: '483acf7e-32db-465c-9210-2327682b6eaf',
    authority: 'https://login.microsoftonline.com/fc406171-ed8a-49b5-8e6c-fcc7f7915c54',
    redirectUri: `${appHost}`,
    postLogoutRedirectUri: `${appHost}/logout`,
  },
  api: {
    endpoint: `${apiHost}/api`,
    scopes: ['api://483acf7e-32db-465c-9210-2327682b6eaf/app'],
    swaggerUri: `${apiHost}/`,
  },
  profile: {
    endpoint: 'https://graph.microsoft.com/v1.0/me',
    scopes: ['user.read'],
  },
};
