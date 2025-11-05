<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>

  <xsl:key name="col" match="DocumentElement/RASImport[1]/*" use="name()" />

  <xsl:template match="/DocumentElement">

    <xsl:variable name = "varBPSPercentage">
			<xsl:value-of select="'Percentage'"/>
	</xsl:variable>

    <xsl:copy>
      <xsl:for-each select="RASImport[position() > 1]">
        <xsl:variable name="varSedol" select="COL1" />
        
        <xsl:for-each select="*[not(self::COL1)]">

			<xsl:variable name="varQuantity">
				<xsl:value-of select="substring-before(.,'%')"/>
			</xsl:variable>

			<xsl:variable name="varQuantitySet">
				<!--<xsl:value-of select="."/>-->
				<xsl:value-of select="substring-before(.,'%')"/>
			</xsl:variable>

			<xsl:variable name ="varAccount">
				<xsl:value-of select="normalize-space(key('col', name()))"/>
			</xsl:variable>

		<xsl:if test="((number($varQuantity) or contains($varQuantitySet,'&amp;')) and $varQuantity !='*')">
			<RASImport>

				 <Symbol>
					<xsl:choose>
							<xsl:when test="$varSedol = '5962280' ">
								<xsl:value-of select="'LISP-SWX'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '4409205' ">
								<xsl:value-of select="'FIE-XET'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'BQ7ZV06' ">
								<xsl:value-of select="'STMN-SWX'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2459202' ">
								<xsl:value-of select="'IDXX'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'BYY88Y7' ">
								<xsl:value-of select="'GOOG'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'BYVY8G0' ">
								<xsl:value-of select="'GOOGL'"/>
							</xsl:when>							
							<xsl:when test="$varSedol = 'BSHZ3Q0' ">
								<xsl:value-of select="'TECH'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'B142S60' ">
								<xsl:value-of select="'KNIN-SWX'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'BCRWZ18' ">
								<xsl:value-of select="'CFR-SWX'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'B95WG16' ">
								<xsl:value-of select="'ZTS'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '7333378' ">
								<xsl:value-of select="'LONN-SWX'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '5253973' ">
								<xsl:value-of select="'RMS-EEB'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2886907' ">
								<xsl:value-of select="'TMO'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'BD8FDD1' ">
								<xsl:value-of select="'TTD'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2073390' ">
								<xsl:value-of select="'BRK.B'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2065159' ">
								<xsl:value-of select="'ADSK'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'BF2DSG3' ">
								<xsl:value-of select="'SIKA-SWX'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'BNZGVV1' ">
								<xsl:value-of select="'UMG-EEB'"/>
							</xsl:when>		
							<xsl:when test="$varSedol = '7212477' ">
								<xsl:value-of select="'EL-EEB'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2650050' ">
								<xsl:value-of select="'EMA-TC'"/>
							</xsl:when>
							<xsl:when test="$varSedol = '2046251' ">
								<xsl:value-of select="'AAPL'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2740542' ">
								<xsl:value-of select="'ACGL'"/>
							</xsl:when>
							<xsl:when test="$varSedol = 'B4BNMY3' ">
								<xsl:value-of select="'ACN'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'BM8XG06' ">
								<xsl:value-of select="'ACTV'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2897404' ">
								<xsl:value-of select="'AGG'"/>
							</xsl:when>
							<xsl:when test="$varSedol = '2019952' ">
								<xsl:value-of select="'ALL'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'B0J7D57' ">
								<xsl:value-of select="'AMP'"/>
							</xsl:when>
							<xsl:when test="$varSedol = 'BKDZSL8' ">
								<xsl:value-of select="'AVDV'"/>
							</xsl:when>
							<xsl:when test="$varSedol = 'BKF2SL7' ">
								<xsl:value-of select="'BX'"/>
							</xsl:when>
							<xsl:when test="$varSedol = 'BTGQCX1' ">
								<xsl:value-of select="'CCI'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'B3F3PJ8' ">
								<xsl:value-of select="'CFRUY'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2795393' ">
								<xsl:value-of select="'COR'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2310525' ">
								<xsl:value-of select="'CRM'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'BNR5PK3' ">
								<xsl:value-of select="'DFUS'"/>
							</xsl:when>
							<xsl:when test="$varSedol = '2250870' ">
								<xsl:value-of select="'DHR'"/>
							</xsl:when>
							<xsl:when test="$varSedol = 'BLSPXB5' ">
								<xsl:value-of select="'DYLD'"/>
							</xsl:when>
							<xsl:when test="$varSedol = 'B2PXT11' ">
								<xsl:value-of select="'EPI'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2373777' ">
								<xsl:value-of select="'EWH'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'BNDPYV1' ">
								<xsl:value-of select="'ERF-EEB'"/>
							</xsl:when>
							<xsl:when test="$varSedol = '2246039' ">
								<xsl:value-of select="'FMX'"/>
							</xsl:when>
							<xsl:when test="$varSedol = 'B90VK61' ">
								<xsl:value-of select="'FPE'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'B4WHY15' ">
								<xsl:value-of select="'FRCB'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'B1MYR16' ">
								<xsl:value-of select="'GBF'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2380443' ">
								<xsl:value-of select="'GGG'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2367026' ">
								<xsl:value-of select="'GIS'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'B1MYR38' ">
								<xsl:value-of select="'GVI'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'BMGC490' ">
								<xsl:value-of select="'HACK'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2781648' ">
								<xsl:value-of select="'HDB'"/>
							</xsl:when>
							<xsl:when test="$varSedol = 'B3F7NR4' ">
								<xsl:value-of select="'HESAY'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2569286' ">
								<xsl:value-of select="'IBN'"/>
							</xsl:when>
							<xsl:when test="$varSedol = 'BYZFXF3' ">
								<xsl:value-of select="'ICVT'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'B8NDCB6' ">
								<xsl:value-of select="'IEMG'"/>
							</xsl:when>
							<xsl:when test="$varSedol = '2678869' ">
								<xsl:value-of select="'IJR'"/>
							</xsl:when>
							<xsl:when test="$varSedol = '2593025' ">
								<xsl:value-of select="'IVV'"/>
							</xsl:when>
							<xsl:when test="$varSedol = 'BNGC0D3' ">
								<xsl:value-of select="'J'"/>
							</xsl:when>
							<xsl:when test="$varSedol = '2475833' ">
								<xsl:value-of select="'JNJ'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2190385' ">
								<xsl:value-of select="'JPM'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'B3KG633' ">
								<xsl:value-of select="'KHNGY'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'BQ84ZQ6' ">
								<xsl:value-of select="'KVUE'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'BG10D45' ">
								<xsl:value-of select="'LSAF'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'BM8XG17' ">
								<xsl:value-of select="'LSAT'"/>
							</xsl:when>		
							<xsl:when test="$varSedol = 'BJT1RW7' ">
								<xsl:value-of select="'LYFT'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'B121557' ">
								<xsl:value-of select="'MA'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2778844' ">
								<xsl:value-of select="'MRK'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2588173' ">
								<xsl:value-of select="'MSFT'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2126249' ">
								<xsl:value-of select="'MTD'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2857817' ">
								<xsl:value-of select="'NFLX'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'B014JG9' ">
								<xsl:value-of select="'NSRGY'"/>
							</xsl:when>
							<xsl:when test="$varSedol = 'BLDC8J4' ">
								<xsl:value-of select="'OGN'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2130109' ">
								<xsl:value-of select="'OKE'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'B65LWX6' ">
								<xsl:value-of select="'ORLY'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'B1FLNF0' ">
								<xsl:value-of select="'PFF'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2704407' ">
								<xsl:value-of select="'PG'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'B44WZD7' ">
								<xsl:value-of select="'PLD'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'BDQYP67' ">
								<xsl:value-of select="'QQQ'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'B3KG688' ">
								<xsl:value-of select="'SAUHY'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2779397' ">
								<xsl:value-of select="'SCHW'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2971524' ">
								<xsl:value-of select="'SHY'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'BD8DJ71' ">
								<xsl:value-of select="'SNAP'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'B1Y9HY2' ">
								<xsl:value-of select="'SPTL'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2840215' ">
								<xsl:value-of select="'SPY'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'BMWRCN9' ">
								<xsl:value-of select="'SQEW'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'B1MYR05' ">
								<xsl:value-of select="'USIG'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'B2PZN04' ">
								<xsl:value-of select="'V'"/>
							</xsl:when>		
							<xsl:when test="$varSedol = 'B23MX41' ">
								<xsl:value-of select="'VEA'"/>
							</xsl:when>
							<xsl:when test="$varSedol = 'BF2GMJ3' ">
								<xsl:value-of select="'VOO'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'B1CL9M2' ">
								<xsl:value-of select="'VOT'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2762568' ">
								<xsl:value-of select="'VTI'"/>
							</xsl:when>		
							<xsl:when test="$varSedol = '2353843' ">
								<xsl:value-of select="'VUG'"/>
							</xsl:when>		
							<xsl:when test="$varSedol = '2955733' ">
								<xsl:value-of select="'WAB'"/>
							</xsl:when>		
							<xsl:when test="$varSedol = '2950482' ">
								<xsl:value-of select="'WST'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'BLCF3J9' ">
								<xsl:value-of select="'WTRG'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'BK71LY6' ">
								<xsl:value-of select="'WW'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'B0Y90K2' ">
								<xsl:value-of select="'XBI'"/>
							</xsl:when>		
							<xsl:when test="$varSedol = '2402466' ">
								<xsl:value-of select="'XLE'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2369709' ">
								<xsl:value-of select="'XLK'"/>
							</xsl:when>		
							<xsl:when test="$varSedol = 'B17N7R4' ">
								<xsl:value-of select="'XRT'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = 'BPGMZQ5' ">
								<xsl:value-of select="'VLTO'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '2091930' ">
								<xsl:value-of select="'2091930'"/>
							</xsl:when>
							<xsl:when test="$varSedol = '2467246' ">
								<xsl:value-of select="'2467246'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '4834108' ">
								<xsl:value-of select="'SU-EEB'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '6821506' ">
								<xsl:value-of select="'6758-TSE'"/>
							</xsl:when>		
							<xsl:when test="$varSedol = '4057808' ">
								<xsl:value-of select="'OR-EEB'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '0237400' ">
								<xsl:value-of select="'DGE-LON'"/>
							</xsl:when>	
							<xsl:when test="$varSedol = '6441506' ">
								<xsl:value-of select="'7741-TSE'"/>
							</xsl:when>							
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>						
			</Symbol>           

					
					<AccountOrGroupName>
						<xsl:choose>
							<xsl:when test="$varAccount = '44320085' ">
								<xsl:value-of select="'BW - AUSTIN SMITH'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = 'Irwin - Evercore' ">
								<xsl:value-of select="'Irwin - Evercore'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = 'SURS - NT' ">
								<xsl:value-of select="'SURS - NT'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '109595' ">
								<xsl:value-of select="'MassPrim - BNY'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '93913500' ">
								<xsl:value-of select="'PAMELA MANICE'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '51803242' ">
								<xsl:value-of select="'ROSEMARIE MORSE'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '36730222' ">
								<xsl:value-of select="'H HOOD &amp; J HOOD TTEE'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '18777595' ">
								<xsl:value-of select="'DARREN CLAY SEIRER'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '37565889' ">
								<xsl:value-of select="'RIAD KHALIL RIZK FAMILY LLC'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '42870187' ">
								<xsl:value-of select="'LAFOREST'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '33526731' ">
								<xsl:value-of select="'JOHN H MANICE'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '92853527' ">
								<xsl:value-of select="'JAMES M HARDIGG'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '28071794' ">
								<xsl:value-of select="'J HOOD &amp; M THOMAS TTEE'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '85593506' ">
								<xsl:value-of select="'BOZEMAN 1'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '35327443' ">
								<xsl:value-of select="'OAK HILL'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '12618007' ">
								<xsl:value-of select="'STEPHEN AN YUEH CHEN'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '62678991' ">
								<xsl:value-of select="'MODEL PORTFOLIO'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '13604907' ">
								<xsl:value-of select="'ROBERT S SCHACHTER'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '72241697' ">
								<xsl:value-of select="'H KIRWAN-TAYLOR &amp;'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '96637857' ">
								<xsl:value-of select="'GENEVIEVE P HARDIGG &amp;'"/>
							</xsl:when>		
							<xsl:when test="$varAccount = '28546042' ">
								<xsl:value-of select="'SUZANNE JS DAVIDSON IRA'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '53053222' ">
								<xsl:value-of select="'LAURA LINDLEY BROCK TTEE'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '61740685' ">
								<xsl:value-of select="'ADAM D SENDER'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '33219634' ">
								<xsl:value-of select="'HOLLI P THOMPSON'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '56617917' ">
								<xsl:value-of select="'KATHERINE FC CARY CUST FOR'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '84498567' ">
								<xsl:value-of select="'TIMOTHY COX MEDLEY TTEE'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '59052841' ">
								<xsl:value-of select="'KENTLANDS'"/>
							</xsl:when>		
							<xsl:when test="$varAccount = '48151442' ">
								<xsl:value-of select="'ANTONINA'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '44452207' ">
								<xsl:value-of select="'BARBARA M HENAGAN'"/>
							</xsl:when>		
							<xsl:when test="$varAccount = '14483346' ">
								<xsl:value-of select="'HANS-PETER GRUENIG'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '53851029' ">
								<xsl:value-of select="'JOHN SHU NAN CHEN'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '55337992' ">
								<xsl:value-of select="'HELEN T KLEBNIKOV'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '55876674' ">
								<xsl:value-of select="'WILLIAM MEDLEY'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '42073689' ">
								<xsl:value-of select="'HILARY ESTIN HOOD 42073689'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '24640282' ">
								<xsl:value-of select="'OLD - EMILEE FAMILY INVESTMENTS LLC'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '97949375' ">
								<xsl:value-of select="'REBECCA ABRAMS'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '82185117' ">
								<xsl:value-of select="'JENNIFER ASARALI'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '31474814' ">
								<xsl:value-of select="'SIENA HOOD 31474814'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '74749015' ">
								<xsl:value-of select="'BOZEMAN SEP'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '78840239' ">
								<xsl:value-of select="'PETER DARIN PROZES'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '41920312' ">
								<xsl:value-of select="'LILA HOOD'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '73620655' ">
								<xsl:value-of select="'NIMROD DAVID PFEFFER'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '30052608' ">
								<xsl:value-of select="'BOZEMAN ROTH'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '91339053' ">
								<xsl:value-of select="'SUZANNE JS DAVIDSON TOD'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '91339053' ">
								<xsl:value-of select="'WHITNEY HOOD 71310435'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '77421606' ">
								<xsl:value-of select="'PETER KLEBNIKOV &amp;'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '26782350' ">
								<xsl:value-of select="'STEVEN JASON FOX'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '22922335' ">
								<xsl:value-of select="'CONSTANTIN'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '36662146' ">
								<xsl:value-of select="'SPARTACUS'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '90187017' ">
								<xsl:value-of select="'KATHERINE FC CARY'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '65427562' ">
								<xsl:value-of select="'LILA ESTIN HOOD'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '20427018' ">
								<xsl:value-of select="'ALEXANDER KLEBNIKOV'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '40766827' ">
								<xsl:value-of select="'MICHAEL ADAMS CUNNINGHAM'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '87017280' ">
								<xsl:value-of select="'SULLIVAN'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '74695505' ">
								<xsl:value-of select="'IVAN DOMINIC KIRWAN-TAYLOR'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '51747779' ">
								<xsl:value-of select="'BOZEMAN IRA'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '37114665' ">
								<xsl:value-of select="'HORATIO'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '26401033' ">
								<xsl:value-of select="'TINZEL'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '39734414' ">
								<xsl:value-of select="'IMELDA VAQUERO &amp; RAFAEL CAMACHO'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '13293061' ">
								<xsl:value-of select="'BW - WARREN CARLISLE JR &amp; JENNIFER'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '97955729' ">
								<xsl:value-of select="'SIENA HOOD 97955729'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '69484813' ">
								<xsl:value-of select="'WHITNEY HOOD 69484813'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '31388984' ">
								<xsl:value-of select="'TONKS'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '17281663' ">
								<xsl:value-of select="'BW - BRENT BUCHANAN &amp; REBECCA ELISE BUCHANAN'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '26836920' ">
								<xsl:value-of select="'BW - EMILEE FAMILY INVESTMENTS LLC'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '29233360' ">
								<xsl:value-of select="'BW - MICHAEL DENSMORE'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '81913896' ">
								<xsl:value-of select="'Matt Tambellini'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '37456467' ">
								<xsl:value-of select="'HILARY ESTIN HOOD 37456467'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '19158969' ">
								<xsl:value-of select="'BW - Melissa Adams'"/>
							</xsl:when>		
							<xsl:when test="$varAccount = '29387177' ">
								<xsl:value-of select="'BW - BRYAN DOUGLAS RAY TTEE'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '51121487' ">
								<xsl:value-of select="'BW - MICHEAL S SCHMITT &amp;'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '49586538' ">
								<xsl:value-of select="'BW - VICTOR PELLICIER'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '44137047' ">
								<xsl:value-of select="'BW - DAVID MICHAEL KAETZ'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '48823735' ">
								<xsl:value-of select="'BW - PATSY TINSLEY'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '58387227' ">
								<xsl:value-of select="'BW - JEANNINE WILLIAMS'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '63448850' ">
								<xsl:value-of select="'THE TRAIN FOUNDATION INH IRA'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '35905641' ">
								<xsl:value-of select="'BW - MELEA SIEBERT TOD'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '61539839' ">
								<xsl:value-of select="'BW - MELEA SIEBERT INH IRA'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '58157317' ">
								<xsl:value-of select="'JOHN HOOD IRA ROLLOVER'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '70359980' ">
								<xsl:value-of select="'BW - KAY AND GARY THOMAS'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '12457216' ">
								<xsl:value-of select="'Mark Thomas Architects'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '4426863' ">
								<xsl:value-of select="'Cambridge - F and F'"/>
							</xsl:when>
							<xsl:when test="$varAccount = '52219945' ">
								<xsl:value-of select="'Global - BW - Buchanan DAF'"/>
							</xsl:when>	
							<xsl:when test="$varAccount = '67675099' ">
								<xsl:value-of select="'Global - BW - Carlisle DAF'"/>
							</xsl:when>								
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</AccountOrGroupName>


				<IncreaseDecreaseOrSet>
					<xsl:choose>
						<xsl:when test="contains($varQuantitySet,'&amp;')">
							<xsl:value-of select="'Set'"/>
						</xsl:when>
						<xsl:when test="contains($varQuantitySet,'&amp;') and number($varQuantity) &lt; 0">
							<xsl:value-of select="'Set'"/>
						</xsl:when>
						<xsl:when test="number($varQuantity) and number($varQuantity) &lt; 0">
							<xsl:value-of select="'Decrease'"/>
						</xsl:when>
						<xsl:when test="number($varQuantity) and number($varQuantity) &gt; 0">
							<xsl:value-of select="'Increase'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="''"/>
						</xsl:otherwise>
					</xsl:choose>
				</IncreaseDecreaseOrSet>

                  <BPSOrPercentage>
						<xsl:value-of select="$varBPSPercentage"/>
					</BPSOrPercentage>

					<Target>
					<xsl:choose>
						<xsl:when test="contains($varQuantitySet,'&amp;')">
							<xsl:value-of select="substring-after($varQuantitySet,'&amp;')"/>
						</xsl:when>					
						<xsl:otherwise>
							<xsl:value-of select="$varQuantity"/>
						</xsl:otherwise>
					</xsl:choose>
					</Target>

            </RASImport>
			</xsl:if>
		</xsl:for-each>
      </xsl:for-each>
    </xsl:copy>
  </xsl:template>

</xsl:stylesheet>