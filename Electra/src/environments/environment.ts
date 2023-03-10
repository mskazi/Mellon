// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.
const appHost = 'http://localhost:4200';
const apiHost = 'http://localhost:54888';

export const environment = {
  production: false,
  baseUrl: '',
  useHash: false,
  auth: {
    clientId: '483acf7e-32db-465c-9210-2327682b6eaf',
    authority: 'https://login.microsoftonline.com/fc406171-ed8a-49b5-8e6c-fcc7f7915c54',
    clientSecret: 'hmS8Q~TR67wEEFT1v49i4VcPOOjc-c-JV4UCtcgb',
    redirectUri: `${appHost}`,
    postLogoutRedirectUri: `${appHost}/logout`,
  },
  api: {
    endpoint: `${apiHost}`,
    scopes: ['api://483acf7e-32db-465c-9210-2327682b6eaf/app'],
    swaggerUri: `${apiHost}/`,
  },
  profile: {
    endpoint: 'https://graph.microsoft.com/v1.0/me',
    scopes: ['user.read'],
  },
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
