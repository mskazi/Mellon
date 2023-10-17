import {
  MsalInterceptorConfiguration,
  MsalGuardConfiguration,
  MsalGuard,
} from '@azure/msal-angular';
import {
  IPublicClientApplication,
  PublicClientApplication,
  BrowserCacheLocation,
  LogLevel,
  InteractionType,
} from '@azure/msal-browser';
import { environment } from '@env/environment';

const isIE =
  window.navigator.userAgent.indexOf('MSIE ') > -1 ||
  window.navigator.userAgent.indexOf('Trident/') > -1; // Remove these lines to use Angular Universal

export function MSALInstanceFactory(): IPublicClientApplication {
  return new PublicClientApplication({
    auth: environment.auth,
    cache: {
      cacheLocation: BrowserCacheLocation.LocalStorage,
      storeAuthStateInCookie: isIE, // set to true for IE 11. Remove this line to use Angular Universal
    },
    system: {
      loggerOptions: {
        loggerCallback: (logLevel: LogLevel, message: string) => {
          console.log(message);
        },
        logLevel: LogLevel.Warning,
        piiLoggingEnabled: true,
      },
    },
  });
}

export function MSALInterceptorConfigFactory(): MsalInterceptorConfiguration {
  const protectedResourceMap = new Map<string, Array<string>>();
  protectedResourceMap.set(environment.profile.endpoint, environment.profile.scopes);
  protectedResourceMap.set(environment.api.endpoint, environment.api.scopes);

  return {
    interactionType: InteractionType.Redirect,
    protectedResourceMap,
  };
}

export function MSALGuardConfigFactory(): MsalGuardConfiguration {
  return {
    interactionType: InteractionType.Redirect,
    authRequest: {
      scopes: environment.api.scopes,
    },
    loginFailedRoute: '/403',
  };
}

export const UserGuard = MsalGuard;
