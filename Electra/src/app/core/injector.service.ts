import { Injectable, Injector } from '@angular/core';
/**
 * Hack to pass to all classes not set as INJECTABLE the injector service to load any ROOT service
 */
@Injectable({
  providedIn: 'root',
})
export class InjectorService {
  // set injector as static, set on application module loaded
  public static injector: Injector;
}
