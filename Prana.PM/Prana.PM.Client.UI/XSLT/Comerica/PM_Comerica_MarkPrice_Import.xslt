<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">
		public string Now(int year, int month)
		{
		DateTime thirdFriday= new DateTime(year, month, 15);
		while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
		{
		thirdFriday = thirdFriday.AddDays(1);
		}
		return thirdFriday.ToString();
		}
	</msxsl:script>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>

		<!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
	</xsl:template>

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=6 ">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=6 ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'X'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>


	<xsl:template name="MonthCodeVar">
		<xsl:param name="varMonth"/>
		<xsl:choose>
			<xsl:when test="$varMonth='JAN'">
				<xsl:value-of select="'F'"/>
			</xsl:when>
			<xsl:when test="$varMonth='FEB'">
				<xsl:value-of select="'G'"/>
			</xsl:when>
			<xsl:when test="$varMonth='MAR'">
				<xsl:value-of select="'H'"/>
			</xsl:when>
			<xsl:when test="$varMonth='APR'">
				<xsl:value-of select="'J'"/>
			</xsl:when>
			<xsl:when test="$varMonth='MAY'">
				<xsl:value-of select="'K'"/>
			</xsl:when>
			<xsl:when test="$varMonth='JUN'">
				<xsl:value-of select="'M'"/>
			</xsl:when>
			<xsl:when test="$varMonth='JUL'">
				<xsl:value-of select="'N'"/>
			</xsl:when>
			<xsl:when test="$varMonth='AUG'">
				<xsl:value-of select="'Q'"/>
			</xsl:when>
			<xsl:when test="$varMonth='SEP'">
				<xsl:value-of select="'U'"/>
			</xsl:when>
			<xsl:when test="$varMonth='OCT'">
				<xsl:value-of select="'V'"/>
			</xsl:when>
			<xsl:when test="$varMonth='NOV'">
				<xsl:value-of select="'X'"/>
			</xsl:when>
			<xsl:when test="$varMonth='DEC'">
				<xsl:value-of select="'Z'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


	<xsl:template name="Future">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="contains(COL9,'FUT')">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(COL5,' ')"/>
			</xsl:variable>

			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring-before(substring-after(normalize-space(COL5),' '),' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring-after(substring-after(normalize-space(COL5),' '),' ')"/>
			</xsl:variable>

			<xsl:variable name="MonthCode">
				<xsl:call-template name="MonthCodeVar">
					<xsl:with-param name="varMonth" select="$ExpiryMonth"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:value-of select="concat($UnderlyingSymbol,' ',$MonthCode,$ExpiryYear)"/>
		</xsl:if>
	</xsl:template>




	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="contains(substring-after(substring-after(substring-after(COL8,' '),' '),' '),C) or contains(substring-after(substring-after(substring-after(COL11,' '),' '),' '),P) ">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(COL8,' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring(substring-after(COL8,' '),1,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring(substring-after(COL8,' '),3,4)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(COL8,' '),6,7)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring-after(substring-after(substring-after(COL8,' '),' '),' ')"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<!--<xsl:value-of select =" substring-before(substring-after(substring-after(normalize-space(COL8),' '),' '),' ')"/>-->
				<xsl:value-of select="format-number(substring-before(substring-after(substring-after(normalize-space(COL8),' '),' '),' '),'#.0')"/>
			</xsl:variable>


			<xsl:variable name="MonthCodVar">
				<xsl:call-template name="MonthCode">
					<xsl:with-param name="Month" select="number($ExpiryMonth)"/>
					<xsl:with-param name="PutOrCall" select="$PutORCall"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="Day">
				<xsl:choose>
					<xsl:when test="substring($ExpiryDay,1,1)='0'">
						<xsl:value-of select="substring($ExpiryDay,2,1)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$ExpiryDay"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="ThirdFriday">
				<xsl:choose>
					<xsl:when test="number($ExpiryMonth) and number($ExpiryYear)">
						<xsl:value-of select="my:Now(number($ExpiryYear),number($ExpiryMonth))"/>
					</xsl:when>
				</xsl:choose>
			</xsl:variable>
			<!--<xsl:choose>
				<xsl:when test="number($ExpiryMonth)=1 and $ExpiryYear='15'">

				</xsl:when>

				<xsl:otherwise>-->
			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)"/>
			<!--</xsl:otherwise>-->
			<!--

			</xsl:choose>-->
		</xsl:if>
	</xsl:template>
	<xsl:template name="spaces">
		<xsl:param name="count"/>
		<xsl:if test="number($count)">
			<xsl:call-template name="spaces">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:value-of select="' '"/>
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>
			
			<xsl:variable name="FundName" select="substring-before(substring-after(//PositionMaster[contains(COL1,'Account')]/COL1, ':'),'-')"/>

			<xsl:variable name="Date" select="substring-after(//PositionMaster[contains(COL1,'As of Date')]/COL1, ':')"/>



			<xsl:for-each select ="//PositionMaster">



				<xsl:variable name="MarkPrice">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL8"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($MarkPrice)">



					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Whitman'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL3"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						


						<xsl:variable name="Symbol">
							<xsl:value-of select="COL1"/>
						</xsl:variable>

						<Symbol>


							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>

						</Symbol>

						<MarkPrice>
							<xsl:choose>
								<xsl:when test="$MarkPrice &gt; 0">
									<xsl:value-of select="$MarkPrice"/>

								</xsl:when>
								<xsl:when test="$MarkPrice &lt; 0">
									<xsl:value-of select="$MarkPrice * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarkPrice>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<Date>
							<xsl:value-of select="''"/>
						</Date>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>