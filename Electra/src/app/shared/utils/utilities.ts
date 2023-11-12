import { UUID } from 'angular2-uuid';
import * as _ from 'lodash';
import { Subject, throwError } from 'rxjs';
import { HttpResponse } from '@angular/common/http';
import { DatePipe } from '@angular/common';
import { ExportFormatTypes } from '@core';
import * as XLSX from 'xlsx';
import { PaginatedListParams } from '@core/core-model';

// Utilities is a functional class that contains only static methods;
export class Utilities {
  // Auto generates a Unique Identifier
  static GenGUI(): string {
    return UUID.UUID();
  }

  static emptyObject(obj: any) {
    return Object.values(obj).every(
      (x: any) =>
        x === null ||
        x === '' ||
        x === undefined ||
        _.isEmpty(x) ||
        (_.has(x, 'start') && (x.start === null || x.start === undefined))
    );
  }

  static emptyObjectWithoutRangeDates(obj: any) {
    return Object.values(obj).every((x: any) => x === null || x === '' || x === undefined);
  }

  static allowArrowNavigation($event: any) {
    if ($event.key === 'Enter' || $event.key === 'ArrowDown' || $event.key === 'ArrowUp') {
      return;
    }
    $event.stopPropagation();
  }

  /**
   * Method to order select field options with normalized german characters
   * @param options the selection list
   * @param property the property with which the ordering will be triggered
   * @param ordering the order
   */
  static orderNormalized(options: any[], property: string, ordering: any[]) {
    let normalizedOptions = Utilities.normalizeOptionsProperty(options, property);
    return _.orderBy(normalizedOptions, ['normalizedText'], ordering);
  }

  private static normalizeOptionsProperty(options: any[], property: string) {
    let normalizedOptions: any[] = [];
    _.forEach(options, (option: any) => {
      option['normalizedText'] = this.normalizeCharacters(option[property]);
      normalizedOptions.push(option);
    });
    return normalizedOptions;
  }

  private static normalizeCharacters(text: string): string {
    if (text && text.length > 0) {
      let loweCaseText = text.toLocaleLowerCase();
      let normalizedText = '';
      _.forEach(loweCaseText, normalizedChar => {
        switch (normalizedChar) {
          case 'ß':
            normalizedText += 's';
            break;
          case 'ä':
          case 'à':
          case 'á':
          case 'â':
          case 'ã':
          case 'å':
            normalizedText += 'a';
            break;
          case 'ö':
          case 'ô':
          case 'ð':
          case 'ò':
          case 'ó':
          case 'õ':
            normalizedText += 'o';
            break;
          case 'ü':
          case 'ù':
          case 'û':
          case 'ú':
            normalizedText += 'u';
            break;
          case 'è':
          case 'é':
          case 'ê':
          case 'ë':
            normalizedText += 'e';
            break;
          case 'ç':
            normalizedText += 'c';
            break;
          case 'î':
          case 'ì':
          case 'í':
          case 'ï':
            normalizedText = 'i';
            break;
          default:
            normalizedText += normalizedChar;
            break;
        }
      });
      return normalizedText;
    }
    return text;
  }

  static customRoundNumber(num: number | null): number {
    if (num === null || isNaN(num)) {
      return 0;
    }
    let diff = num - Math.floor(num * 10) / 10; //round diff to 6th decimal in order to keep 3rd decimal unchanged from rounding
    if (diff < 0.025) {
      num = Math.floor(num * 10) / 10;
    } else if (diff >= 0.025 && diff < 0.075) {
      num = Math.floor(num * 10) / 10 + 0.05;
    } else {
      num = Math.floor(num * 10) / 10 + 0.1;
    }
    return num;
  }

  static extractErrorsResponse(errorResponse: any) {
    if (errorResponse.status === 400 && errorResponse.error && _.isArray(errorResponse.error)) {
      return _.uniqBy(errorResponse.error, 'message');
    }
    return [];
  }

  static catchError(errorResponse: any, errorSubject: Subject<any>) {
    errorSubject.next(Utilities.extractErrorsResponse(errorResponse));
    return throwError(() => new Error(errorResponse));
  }

  static downloadFile(file: Blob, fileName: string) {
    const url = window.URL.createObjectURL(file);
    const link = document.createElement('a');
    link.href = url;
    link.download = fileName;
    link.click();
    window.URL.revokeObjectURL(url);
    link.remove();
  }

  static downloadFileOnNewTab(file: Blob, fileName: string) {
    var url = window.URL.createObjectURL(file);
    var anchor = document.createElement('a');
    anchor.href = url;
    anchor.target = '_blank';
    anchor.click();
    window.URL.revokeObjectURL(url);
    anchor.remove();
  }

  static alphabeticalOrderTextByProperty(companies: any[], property: string): string {
    let textArray = [''];
    const delimiter = ' / ';
    companies = _.orderBy(companies, property);
    companies.forEach(company => {
      textArray.push(company[property]);
    });

    let result = textArray.join(delimiter);
    if (result.startsWith(delimiter)) {
      result = result.substring(delimiter.length);
    }

    return result;
  }

  /**
   *
   * @param text starting text
   * @param data the data from which the text will be created
   * @param sortProperty the order property for the data
   */
  static showJointVenturesSubcontractors(text: string, data: any, sortProperty: string): string {
    if (data.submittent.jointVentures.length > 0) {
      text =
        text +
        ' (ARGE: ' +
        Utilities.alphabeticalOrderTextByProperty(data.submittent.jointVentures, sortProperty);
    }

    if (data.submittent.subcontractors.length > 0) {
      if (data.submittent.jointVentures.length > 0) {
        text = text + ', Sub. U: ';
      } else {
        text = text + ' (Sub. U: ';
      }
      text =
        text +
        Utilities.alphabeticalOrderTextByProperty(data.submittent.subcontractors, sortProperty);
    }

    if (data.submittent.jointVentures.length > 0 || data.submittent.subcontractors.length > 0) {
      text = text + ')';
    }

    return text;
  }
  /**
   * Return filename from headers
   * @param response
   * @returns
   */
  static exportFileName(response: HttpResponse<any>): string | undefined {
    const fn = response.headers.get('content-disposition') as string;
    if (!fn) {
      return undefined;
    }
    let fileName = fn.split(';')[1].trim().split('=')[1];
    // Remove quotes in order to enable filenames with special characters like comma
    if (fileName.startsWith('"') && fileName.endsWith('"')) {
      fileName = fileName.substring(1, fileName.length - 1);
    }
    return fileName;
  }

  static exportFile(
    dataToSave: any[],
    heading: string[],
    prefixTitle: string,
    datePipe: DatePipe,
    exportFormatType: ExportFormatTypes
  ) {
    /* pass here the table id */
    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(dataToSave);
    /* generate workbook and add the worksheet */
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.sheet_add_aoa(ws, [heading]);
    XLSX.utils.sheet_add_json(ws, dataToSave, { origin: 'A2', skipHeader: true });

    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
    /* save to file */
    //if (exportFormatType === ExportFormatTypes.EXCELX) {
    XLSX.writeFile(wb, Utilities.generateFilename(prefixTitle, datePipe, exportFormatType));
    // } else {

    // }
  }

  static generateFilename(
    prefix: string,
    datePipe: DatePipe,
    exportFormatTypes: ExportFormatTypes
  ) {
    return `${prefix}_${datePipe.transform(
      Date.now(),
      'dd_MM_YYYY_HH_SS',
      'UTC',
      'en'
    )}.${exportFormatTypes}`;
  }

  static paginatedQueryParams(pagingParams?: PaginatedListParams): string {
    const parts = [];
    if (pagingParams?.start) parts.push(`start=${pagingParams.start}`);
    if (pagingParams?.pageSize) parts.push(`pageSize=${pagingParams.pageSize}`);
    if (pagingParams?.search) parts.push(`search=${pagingParams.search}`);
    return parts.join('&');
  }

  static orderQueryParams(pagingParams?: PaginatedListParams): string {
    if (pagingParams && pagingParams.orderBy) {
      const pars: Array<string> = [];
      pagingParams.orderBy.forEach((v, index) => {
        if (v.name && v.dir) {
          pars.push(`OrderBy[${index}]=${v.name} ${v.dir}`);
        }
      });
      return pars.join('&');
    }
    return '';
  }
}
