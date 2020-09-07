import { isArray } from 'util';
import { AnyARecord } from 'dns';

export class TypeSurvey {


	public SurveyPath: string = '';
	public SurveyName: string | null = '';
	public Count: number = 0;
	public Number: number = 0;
	public String: number = 0;
	public Null: number = 0;
	public Undefined: number = 0;
	public Object: number = 0;
	public ObjectTree: Array<TypeSurvey> = [];
	public Array: number = 0;
	public ArrayTree: Array<TypeSurvey> = [];

	constructor(Name: string | null, Path: string) {
		this.SurveyPath = Path;
		this.SurveyName = Name;
	}

	public Tally(Value: any, Name: string | null = null) {
		this.Count++;

		if (Value == undefined) {
			this.Undefined++;
		}
		else if (Value == null) {
			this.Null++;
		}
		else {
			var DaType = typeof Value;

			if (DaType == 'string') {
				this.String++;
			}
			else if (DaType == 'number') {
				this.Number++;
			}
			else if (Array.isArray(Value)) {
				this.Array++;

				let arr: TypeSurvey = new TypeSurvey(Name, this.SurveyPath);

				arr.Tally(Value);

				this.ArrayTree.unshift(arr);
			}
			else if (DaType == 'object') {
				this.Object++;
			}
		}
	}
}
