﻿/*
 Highcharts JS v7.2.0 (2019-09-03)

 Data module

 (c) 2012-2019 Torstein Honsi

 License: www.highcharts.com/license
*/
(function (b) { "object" === typeof module && module.exports ? (b["default"] = b, module.exports = b) : "function" === typeof define && define.amd ? define("highcharts/modules/data", ["highcharts"], function (q) { b(q); b.Highcharts = q; return b }) : b("undefined" !== typeof Highcharts ? Highcharts : void 0) })(function (b) {
    function q(u, b, q, A) { u.hasOwnProperty(b) || (u[b] = A.apply(null, q)) } b = b ? b._modules : {}; q(b, "mixins/ajax.js", [b["parts/Globals.js"], b["parts/Utilities.js"]], function (u, b) {
        var q = b.objectEach; u.ajax = function (b) {
            var n = u.merge(!0,
                { url: !1, type: "get", dataType: "json", success: !1, error: !1, data: !1, headers: {} }, b); b = { json: "application/json", xml: "application/xml", text: "text/plain", octet: "application/octet-stream" }; var v = new XMLHttpRequest; if (!n.url) return !1; v.open(n.type.toUpperCase(), n.url, !0); n.headers["Content-Type"] || v.setRequestHeader("Content-Type", b[n.dataType] || b.text); q(n.headers, function (b, u) { v.setRequestHeader(u, b) }); v.onreadystatechange = function () {
                    if (4 === v.readyState) {
                        if (200 === v.status) {
                            var b = v.responseText; if ("json" ===
                                n.dataType) try { b = JSON.parse(b) } catch (D) { n.error && n.error(v, D); return } return n.success && n.success(b)
                        } n.error && n.error(v, v.responseText)
                    }
                }; try { n.data = JSON.stringify(n.data) } catch (E) { } v.send(n.data || !0)
        }; u.getJSON = function (b, n) { u.ajax({ url: b, success: n, dataType: "json", headers: { "Content-Type": "text/plain" } }) }
    }); q(b, "modules/data.src.js", [b["parts/Globals.js"], b["parts/Utilities.js"]], function (b, q) {
        var u = q.defined, A = q.isNumber, n = q.objectEach, v = q.splat; q = b.addEvent; var E = b.Chart, D = b.win.document, G = b.pick,
            B = b.merge, H = b.fireEvent, C = function (a, d, c) { this.init(a, d, c) }; b.extend(C.prototype, {
                init: function (a, d, c) {
                    var g = a.decimalPoint; d && (this.chartOptions = d); c && (this.chart = c); "." !== g && "," !== g && (g = void 0); this.options = a; this.columns = a.columns || this.rowsToColumns(a.rows) || []; this.firstRowAsNames = G(a.firstRowAsNames, this.firstRowAsNames, !0); this.decimalRegex = g && new RegExp("^(-?[0-9]+)" + g + "([0-9]+)$"); this.rawColumns = []; if (this.columns.length) { this.dataFound(); var f = !0 } this.hasURLOption(a) && (clearTimeout(this.liveDataTimeout),
                        f = !1); f || (f = this.fetchLiveData()); f || (f = !!this.parseCSV().length); f || (f = !!this.parseTable().length); f || (f = this.parseGoogleSpreadsheet()); !f && a.afterComplete && a.afterComplete()
                }, hasURLOption: function (a) { return !(!a || !(a.rowsURL || a.csvURL || a.columnsURL)) }, getColumnDistribution: function () {
                    var a = this.chartOptions, d = this.options, c = [], g = function (a) { return (b.seriesTypes[a || "line"].prototype.pointArrayMap || [0]).length }, f = a && a.chart && a.chart.type, e = [], m = [], h = 0; d = d && d.seriesMapping || a && a.series && a.series.map(function () { return { x: 0 } }) ||
                        []; var k; (a && a.series || []).forEach(function (a) { e.push(g(a.type || f)) }); d.forEach(function (a) { c.push(a.x || 0) }); 0 === c.length && c.push(0); d.forEach(function (d) { var c = new y, t = e[h] || g(f), r = (a && a.series || [])[h] || {}, x = b.seriesTypes[r.type || f || "line"].prototype.pointArrayMap, z = x || ["y"]; (u(d.x) || r.isCartesian || !x) && c.addColumnReader(d.x, "x"); n(d, function (a, d) { "x" !== d && c.addColumnReader(a, d) }); for (k = 0; k < t; k++)c.hasReader(z[k]) || c.addColumnReader(void 0, z[k]); m.push(c); h++ }); d = b.seriesTypes[f || "line"].prototype.pointArrayMap;
                    void 0 === d && (d = ["y"]); this.valueCount = { global: g(f), xColumns: c, individual: e, seriesBuilders: m, globalPointArrayMap: d }
                }, dataFound: function () { this.options.switchRowsAndColumns && (this.columns = this.rowsToColumns(this.columns)); this.getColumnDistribution(); this.parseTypes(); !1 !== this.parsed() && this.complete() }, parseCSV: function (a) {
                    function d(a, d, c, g) {
                        function f(d) { h = a[d]; p = a[d - 1]; F = a[d + 1] } function e(a) { t.length < r + 1 && t.push([a]); t[r][t[r].length - 1] !== a && t[r].push(a) } function b() {
                            k > w || w > n ? (++w, m = "") : (!isNaN(parseFloat(m)) &&
                                isFinite(m) ? (m = parseFloat(m), e("number")) : isNaN(Date.parse(m)) ? e("string") : (m = m.replace(/\//g, "-"), e("date")), x.length < r + 1 && x.push([]), c || (x[r][d] = m), m = "", ++r, ++w)
                        } var l = 0, h = "", p = "", F = "", m = "", w = 0, r = 0; if (a.trim().length && "#" !== a.trim()[0]) { for (; l < a.length; l++) { f(l); if ("#" === h) { b(); return } if ('"' === h) for (f(++l); l < a.length && ('"' !== h || '"' === p || '"' === F);) { if ('"' !== h || '"' === h && '"' !== p) m += h; f(++l) } else g && g[h] ? g[h](h, m) && b() : h === z ? b() : m += h } b() }
                    } function c(a) {
                        var d = 0, c = 0, g = !1; a.some(function (a, g) {
                            var f = !1,
                                e = ""; if (13 < g) return !0; for (var b = 0; b < a.length; b++) { g = a[b]; var h = a[b + 1]; var m = a[b - 1]; if ("#" === g) break; if ('"' === g) if (f) { if ('"' !== m && '"' !== h) { for (; " " === h && b < a.length;)h = a[++b]; "undefined" !== typeof r[h] && r[h]++; f = !1 } } else f = !0; else "undefined" !== typeof r[g] ? (e = e.trim(), isNaN(Date.parse(e)) ? !isNaN(e) && isFinite(e) || r[g]++ : r[g]++ , e = "") : e += g; "," === g && c++; "." === g && d++ }
                        }); g = r[";"] > r[","] ? ";" : ","; e.decimalPoint || (e.decimalPoint = d > c ? "." : ",", f.decimalRegex = new RegExp("^(-?[0-9]+)" + e.decimalPoint + "([0-9]+)$"));
                        return g
                    } function g(a, d) {
                        var c = [], g = 0, b = !1, h = [], m = [], l; if (!d || d > a.length) d = a.length; for (; g < d; g++)if ("undefined" !== typeof a[g] && a[g] && a[g].length) { var k = a[g].trim().replace(/\//g, " ").replace(/\-/g, " ").replace(/\./g, " ").split(" "); c = ["", "", ""]; for (l = 0; l < k.length; l++)l < c.length && (k[l] = parseInt(k[l], 10), k[l] && (m[l] = !m[l] || m[l] < k[l] ? k[l] : m[l], "undefined" !== typeof h[l] ? h[l] !== k[l] && (h[l] = !1) : h[l] = k[l], 31 < k[l] ? c[l] = 100 > k[l] ? "YY" : "YYYY" : 12 < k[l] && 31 >= k[l] ? (c[l] = "dd", b = !0) : c[l].length || (c[l] = "mm"))) } if (b) {
                            for (l =
                                0; l < h.length; l++)!1 !== h[l] ? 12 < m[l] && "YY" !== c[l] && "YYYY" !== c[l] && (c[l] = "YY") : 12 < m[l] && "mm" === c[l] && (c[l] = "dd"); 3 === c.length && "dd" === c[1] && "dd" === c[2] && (c[2] = "YY"); a = c.join("/"); return (e.dateFormats || f.dateFormats)[a] ? a : (H("deduceDateFailed"), "YYYY/mm/dd")
                        } return "YYYY/mm/dd"
                    } var f = this, e = a || this.options, b = e.csv; a = "undefined" !== typeof e.startRow && e.startRow ? e.startRow : 0; var h = e.endRow || Number.MAX_VALUE, k = "undefined" !== typeof e.startColumn && e.startColumn ? e.startColumn : 0, n = e.endColumn || Number.MAX_VALUE,
                        p = 0, t = [], r = { ",": 0, ";": 0, "\t": 0 }; var x = this.columns = []; b && e.beforeParse && (b = e.beforeParse.call(this, b)); if (b) { b = b.replace(/\r\n/g, "\n").replace(/\r/g, "\n").split(e.lineDelimiter || "\n"); if (!a || 0 > a) a = 0; if (!h || h >= b.length) h = b.length - 1; if (e.itemDelimiter) var z = e.itemDelimiter; else z = null, z = c(b); var w = 0; for (p = a; p <= h; p++)"#" === b[p][0] ? w++ : d(b[p], p - a - w); e.columnTypes && 0 !== e.columnTypes.length || !t.length || !t[0].length || "date" !== t[0][1] || e.dateFormat || (e.dateFormat = g(x[0])); this.dataFound() } return x
                }, parseTable: function () {
                    var a =
                        this.options, d = a.table, c = this.columns, g = a.startRow || 0, f = a.endRow || Number.MAX_VALUE, e = a.startColumn || 0, b = a.endColumn || Number.MAX_VALUE; d && ("string" === typeof d && (d = D.getElementById(d)), [].forEach.call(d.getElementsByTagName("tr"), function (a, d) { d >= g && d <= f && [].forEach.call(a.children, function (a, f) { ("TD" === a.tagName || "TH" === a.tagName) && f >= e && f <= b && (c[f - e] || (c[f - e] = []), c[f - e][d - g] = a.innerHTML) }) }), this.dataFound()); return c
                }, fetchLiveData: function () {
                    function a(k) {
                        function n(h, t, r) {
                            function n() {
                                e && c.liveDataURL ===
                                    h && (d.liveDataTimeout = setTimeout(a, m))
                            } if (!h || 0 !== h.indexOf("http")) return h && g.error && g.error("Invalid URL"), !1; k && (clearTimeout(d.liveDataTimeout), c.liveDataURL = h); b.ajax({ url: h, dataType: r || "json", success: function (a) { c && c.series && t(a); n() }, error: function (a, c) { 3 > ++f && n(); return g.error && g.error(c, a) } }); return !0
                        } n(h.csvURL, function (a) { c.update({ data: { csv: a } }) }, "text") || n(h.rowsURL, function (a) { c.update({ data: { rows: a } }) }) || n(h.columnsURL, function (a) { c.update({ data: { columns: a } }) })
                    } var d = this, c = this.chart,
                        g = this.options, f = 0, e = g.enablePolling, m = 1E3 * (g.dataRefreshRate || 2), h = B(g); if (!this.hasURLOption(g)) return !1; 1E3 > m && (m = 1E3); delete g.csvURL; delete g.rowsURL; delete g.columnsURL; a(!0); return this.hasURLOption(g)
                }, parseGoogleSpreadsheet: function () {
                    function a(d) {
                        var f = ["https://spreadsheets.google.com/feeds/cells", g, e, "public/values?alt=json"].join("/"); b.ajax({
                            url: f, dataType: "json", success: function (g) { d(g); c.enablePolling && setTimeout(function () { a(d) }, 1E3 * (c.dataRefreshRate || 2)) }, error: function (a, d) {
                                return c.error &&
                                    c.error(d, a)
                            }
                        })
                    } var d = this, c = this.options, g = c.googleSpreadsheetKey, f = this.chart, e = c.googleSpreadsheetWorksheet || 1, m = c.startRow || 0, h = c.endRow || Number.MAX_VALUE, k = c.startColumn || 0, n = c.endColumn || Number.MAX_VALUE, p = 1E3 * (c.dataRefreshRate || 2); 4E3 > p && (p = 4E3); g && (delete c.googleSpreadsheetKey, a(function (a) {
                        var c = []; a = a.feed.entry; var g = (a || []).length, e = 0, b; if (!a || 0 === a.length) return !1; for (b = 0; b < g; b++) { var p = a[b]; e = Math.max(e, p.gs$cell.col) } for (b = 0; b < e; b++)b >= k && b <= n && (c[b - k] = []); for (b = 0; b < g; b++) {
                            p = a[b];
                            e = p.gs$cell.row - 1; var q = p.gs$cell.col - 1; if (q >= k && q <= n && e >= m && e <= h) { var t = p.gs$cell || p.content; p = null; t.numericValue ? p = 0 <= t.$t.indexOf("/") || 0 <= t.$t.indexOf("-") ? t.$t : 0 < t.$t.indexOf("%") ? 100 * parseFloat(t.numericValue) : parseFloat(t.numericValue) : t.$t && t.$t.length && (p = t.$t); c[q - k][e - m] = p }
                        } c.forEach(function (a) { for (b = 0; b < a.length; b++)void 0 === a[b] && (a[b] = null) }); f && f.series ? f.update({ data: { columns: c } }) : (d.columns = c, d.dataFound())
                    })); return !1
                }, trim: function (a, d) {
                    "string" === typeof a && (a = a.replace(/^\s+|\s+$/g,
                        ""), d && /^[0-9\s]+$/.test(a) && (a = a.replace(/\s/g, "")), this.decimalRegex && (a = a.replace(this.decimalRegex, "$1.$2"))); return a
                }, parseTypes: function () { for (var a = this.columns, d = a.length; d--;)this.parseColumn(a[d], d) }, parseColumn: function (a, d) {
                    var c = this.rawColumns, g = this.columns, b = a.length, e = this.firstRowAsNames, m = -1 !== this.valueCount.xColumns.indexOf(d), h, k = [], n = this.chartOptions, p, t = (this.options.columnTypes || [])[d]; n = m && (n && n.xAxis && "category" === v(n.xAxis)[0].type || "string" === t); for (c[d] || (c[d] = []); b--;) {
                        var r =
                            k[b] || a[b]; var q = this.trim(r); var u = this.trim(r, !0); var w = parseFloat(u); void 0 === c[d][b] && (c[d][b] = q); n || 0 === b && e ? a[b] = "" + q : +u === w ? (a[b] = w, 31536E6 < w && "float" !== t ? a.isDatetime = !0 : a.isNumeric = !0, void 0 !== a[b + 1] && (p = w > a[b + 1])) : (q && q.length && (h = this.parseDate(r)), m && A(h) && "float" !== t ? (k[b] = r, a[b] = h, a.isDatetime = !0, void 0 !== a[b + 1] && (r = h > a[b + 1], r !== p && void 0 !== p && (this.alternativeFormat ? (this.dateFormat = this.alternativeFormat, b = a.length, this.alternativeFormat = this.dateFormats[this.dateFormat].alternative) :
                                a.unsorted = !0), p = r)) : (a[b] = "" === q ? null : q, 0 !== b && (a.isDatetime || a.isNumeric) && (a.mixed = !0)))
                    } m && a.mixed && (g[d] = c[d]); if (m && p && this.options.sort) for (d = 0; d < g.length; d++)g[d].reverse(), e && g[d].unshift(g[d].pop())
                }, dateFormats: {
                    "YYYY/mm/dd": { regex: /^([0-9]{4})[\-\/\.]([0-9]{1,2})[\-\/\.]([0-9]{1,2})$/, parser: function (a) { return Date.UTC(+a[1], a[2] - 1, +a[3]) } }, "dd/mm/YYYY": { regex: /^([0-9]{1,2})[\-\/\.]([0-9]{1,2})[\-\/\.]([0-9]{4})$/, parser: function (a) { return Date.UTC(+a[3], a[2] - 1, +a[1]) }, alternative: "mm/dd/YYYY" },
                    "mm/dd/YYYY": { regex: /^([0-9]{1,2})[\-\/\.]([0-9]{1,2})[\-\/\.]([0-9]{4})$/, parser: function (a) { return Date.UTC(+a[3], a[1] - 1, +a[2]) } }, "dd/mm/YY": { regex: /^([0-9]{1,2})[\-\/\.]([0-9]{1,2})[\-\/\.]([0-9]{2})$/, parser: function (a) { var d = +a[3]; d = d > (new Date).getFullYear() - 2E3 ? d + 1900 : d + 2E3; return Date.UTC(d, a[2] - 1, +a[1]) }, alternative: "mm/dd/YY" }, "mm/dd/YY": { regex: /^([0-9]{1,2})[\-\/\.]([0-9]{1,2})[\-\/\.]([0-9]{2})$/, parser: function (a) { return Date.UTC(+a[3] + 2E3, a[1] - 1, +a[2]) } }
                }, parseDate: function (a) {
                    var d =
                        this.options.parseDate, c, b = this.options.dateFormat || this.dateFormat, f; if (d) var e = d(a); else if ("string" === typeof a) { if (b) (d = this.dateFormats[b]) || (d = this.dateFormats["YYYY/mm/dd"]), (f = a.match(d.regex)) && (e = d.parser(f)); else for (c in this.dateFormats) if (d = this.dateFormats[c], f = a.match(d.regex)) { this.dateFormat = c; this.alternativeFormat = d.alternative; e = d.parser(f); break } f || (f = Date.parse(a), "object" === typeof f && null !== f && f.getTime ? e = f.getTime() - 6E4 * f.getTimezoneOffset() : A(f) && (e = f - 6E4 * (new Date(f)).getTimezoneOffset())) } return e
                },
                rowsToColumns: function (a) { var d, c; if (a) { var b = []; var f = a.length; for (d = 0; d < f; d++) { var e = a[d].length; for (c = 0; c < e; c++)b[c] || (b[c] = []), b[c][d] = a[d][c] } } return b }, getData: function () { if (this.columns) return this.rowsToColumns(this.columns).slice(1) }, parsed: function () { if (this.options.parsed) return this.options.parsed.call(this, this.columns) }, getFreeIndexes: function (a, d) {
                    var c, b = [], f = []; for (c = 0; c < a; c += 1)b.push(!0); for (a = 0; a < d.length; a += 1) {
                        var e = d[a].getReferencedColumnIndexes(); for (c = 0; c < e.length; c += 1)b[e[c]] =
                            !1
                    } for (c = 0; c < b.length; c += 1)b[c] && f.push(c); return f
                }, complete: function () {
                    var a = this.columns, d, c = this.options, b, f, e = []; if (c.complete || c.afterComplete) {
                        if (this.firstRowAsNames) for (b = 0; b < a.length; b++)a[b].name = a[b].shift(); var m = []; var h = this.getFreeIndexes(a.length, this.valueCount.seriesBuilders); for (b = 0; b < this.valueCount.seriesBuilders.length; b++) { var k = this.valueCount.seriesBuilders[b]; k.populateColumns(h) && e.push(k) } for (; 0 < h.length;) {
                            k = new y; k.addColumnReader(0, "x"); b = h.indexOf(0); -1 !== b && h.splice(b,
                                1); for (b = 0; b < this.valueCount.global; b++)k.addColumnReader(void 0, this.valueCount.globalPointArrayMap[b]); k.populateColumns(h) && e.push(k)
                        } 0 < e.length && 0 < e[0].readers.length && (k = a[e[0].readers[0].columnIndex], void 0 !== k && (k.isDatetime ? d = "datetime" : k.isNumeric || (d = "category"))); if ("category" === d) for (b = 0; b < e.length; b++)for (k = e[b], h = 0; h < k.readers.length; h++)"x" === k.readers[h].configName && (k.readers[h].configName = "name"); for (b = 0; b < e.length; b++) {
                            k = e[b]; h = []; for (f = 0; f < a[0].length; f++)h[f] = k.read(a, f); m[b] =
                                { data: h }; k.name && (m[b].name = k.name); "category" === d && (m[b].turboThreshold = 0)
                        } a = { series: m }; d && (a.xAxis = { type: d }, "category" === d && (a.xAxis.uniqueNames = !1)); c.complete && c.complete(a); c.afterComplete && c.afterComplete(a)
                    }
                }, update: function (a, b) { var c = this.chart; a && (a.afterComplete = function (a) { a && (a.xAxis && c.xAxis[0] && a.xAxis.type === c.xAxis[0].options.type && delete a.xAxis, c.update(a, b, !0)) }, B(!0, c.options.data, a), this.init(c.options.data)) }
            }); b.Data = C; b.data = function (a, b, c) { return new C(a, b, c) }; q(E, "init",
                function (a) { var d = this, c = a.args[0] || {}, g = a.args[1]; c && c.data && !d.hasDataDef && (d.hasDataDef = !0, d.data = new C(b.extend(c.data, { afterComplete: function (a) { var b; if (Object.hasOwnProperty.call(c, "series")) if ("object" === typeof c.series) for (b = Math.max(c.series.length, a && a.series ? a.series.length : 0); b--;) { var f = c.series[b] || {}; c.series[b] = B(f, a && a.series ? a.series[b] : {}) } else delete c.series; c = B(a, c); d.init(c, g) } }), c, d), a.preventDefault()) }); var y = function () { this.readers = []; this.pointIsArray = !0 }; y.prototype.populateColumns =
                    function (a) { var b = !0; this.readers.forEach(function (b) { void 0 === b.columnIndex && (b.columnIndex = a.shift()) }); this.readers.forEach(function (a) { void 0 === a.columnIndex && (b = !1) }); return b }; y.prototype.read = function (a, d) {
                        var c = this.pointIsArray, g = c ? [] : {}; this.readers.forEach(function (e) { var f = a[e.columnIndex][d]; c ? g.push(f) : 0 < e.configName.indexOf(".") ? b.Point.prototype.setNestedProperty(g, f, e.configName) : g[e.configName] = f }); if (void 0 === this.name && 2 <= this.readers.length) {
                            var f = this.getReferencedColumnIndexes();
                            2 <= f.length && (f.shift(), f.sort(function (a, b) { return a - b }), this.name = a[f.shift()].name)
                        } return g
                    }; y.prototype.addColumnReader = function (a, b) { this.readers.push({ columnIndex: a, configName: b }); "x" !== b && "y" !== b && void 0 !== b && (this.pointIsArray = !1) }; y.prototype.getReferencedColumnIndexes = function () { var a, b = []; for (a = 0; a < this.readers.length; a += 1) { var c = this.readers[a]; void 0 !== c.columnIndex && b.push(c.columnIndex) } return b }; y.prototype.hasReader = function (a) {
                        var b; for (b = 0; b < this.readers.length; b += 1) {
                            var c = this.readers[b];
                            if (c.configName === a) return !0
                        }
                    }
    }); q(b, "masters/modules/data.src.js", [], function () { })
});
//# sourceMappingURL=data.js.map