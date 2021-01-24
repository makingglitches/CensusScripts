if (this is BaseRecord)
{
  var obj=(BaseRecord)this;
  sb.Append("BASERECORD(");
}

if (this is Box)
{
  var obj=(Box)this;
  sb.Append("BOX(");
}

if (this is MultiPatchShape)
{
  var obj=(MultiPatchShape)this;
  sb.Append("MULTIPATCH(");
}

if (this is MultiPointMShape)
{
  var obj=(MultiPointMShape)this;
  sb.Append("MULTIPOINTM(");
}

if (this is MultiPointShape)
{
  var obj=(MultiPointShape)this;
  sb.Append("MULTIPOINT(");
}

if (this is MultiPointZShape)
{
  var obj=(MultiPointZShape)this;
  sb.Append("MULTIPOINTZ(");
}

if (this is NullShape)
{
  var obj=(NullShape)this;
  sb.Append("NULL(");
}

if (this is PointMShape)
{
  var obj=(PointMShape)this;
  sb.Append("POINTM(");
}

if (this is PointShape)
{
  var obj=(PointShape)this;
  sb.Append("POINT(");
}

if (this is PointZShape)
{
  var obj=(PointZShape)this;
  sb.Append("POINTZ(");
}

if (this is PolygonMShape)
{
  var obj=(PolygonMShape)this;
  sb.Append("POLYGONM(");
}

if (this is PolygonShape)
{
  var obj=(PolygonShape)this;
  sb.Append("POLYGON(");
}

if (this is PolygonZShape)
{
  var obj=(PolygonZShape)this;
  sb.Append("POLYGONZ(");
}

if (this is PolyLineMShape)
{
  var obj=(PolyLineMShape)this;
  sb.Append("POLYLINEM(");
}

if (this is PolyLineShape)
{
  var obj=(PolyLineShape)this;
  sb.Append("POLYLINE(");
}

if (this is PolyLineZShape)
{
  var obj=(PolyLineZShape)this;
  sb.Append("POLYLINEZ(");
}

if (this is RecordHeader)
{
  var obj=(RecordHeader)this;
  sb.Append("RECORDHEADER(");
}

if (this is ShapeFile)
{
  var obj=(ShapeFile)this;
  sb.Append("FILE(");
}

if (this is SHPHeader)
{
  var obj=(SHPHeader)this;
  sb.Append("SHPHEADER(");
}

if (this is ShpPoint)
{
  var obj=(ShpPoint)this;
  sb.Append("SHPPOINT(");
}

