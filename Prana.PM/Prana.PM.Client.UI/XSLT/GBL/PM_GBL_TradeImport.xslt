<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">

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



	<xsl:template name="MonthCodesVar">
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

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="contains(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL6),' '),' '),' '),' '),' '),C) or contains(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL6),' '),' '),' '),' '),' '),P) ">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(COL6,' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL6),' '),' '),' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring-before(substring-after(normalize-space(COL6),' '),' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL6),' '),' '),' '),' '),3,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL6),' '),' '),' '),' '),' ')"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<!--<xsl:value-of select =" substring-before(substring-after(substring-after(normalize-space(COL8),' '),' '),' ')"/>-->
				<xsl:value-of select="format-number(substring-before(substring-after(substring-after(substring-after(substring-after(normalize-space(COL6),' '),' '),' '),' '),' '),'#.00')"/>
			</xsl:variable>


			<xsl:variable name="MonthCode">
				<xsl:call-template name="MonthCodesVar">
					<xsl:with-param name="varMonth" select="$ExpiryMonth"/>
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
			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCode,$StrikePrice,'D',$Day)"/>
			<!--</xsl:otherwise>-->
			<!--

			</xsl:choose>-->
		</xsl:if>
	</xsl:template>






	<xsl:template name="FutureOption">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="contains(COL2,'FUTFOP')">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring(substring-before(substring-after(substring-after(substring-after(substring-after(normalize-space(COL6),' '),' '),' '),' '),' '),3,3)"/>
			</xsl:variable>

			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring-before(substring-after(normalize-space(COL6),' '),' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-before(substring-after(substring-after(normalize-space(COL6),' '),' '),' '),2,1)"/>
			</xsl:variable>
			<xsl:variable name="PutOrCall">
				<xsl:value-of select="substring(substring-before(COL6,' '),1,1)"/>
			</xsl:variable>

			<xsl:variable name="StrikePrice">

				<xsl:value-of select="format-number(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL6),' '),' '),' '),' '),' '),' '),'#.00')"/>

			</xsl:variable>

			<xsl:variable name="MonthCode">
				<xsl:call-template name="MonthCodesVar">
					<xsl:with-param name="varMonth" select="$ExpiryMonth"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:value-of select="concat($UnderlyingSymbol,' ',$MonthCode,$ExpiryYear,$PutOrCall,$StrikePrice)"/>
		</xsl:if>
	</xsl:template>



	<xsl:template name="FuturesOption">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="contains(COL2,'FUTFOP')">
			<xsl:variable name="UnderlyingSymbol">
				<!--<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(normalize-space(COL6),' '),' '),' '),' ')"/>-->
				<!--<xsl:value-of select="substring(substring-before(substring-after(normalize-space(COL31),' '),' '),2,2)"/>-->
				<xsl:value-of select="concat(substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL6),' '),' '),' '),' '),1,1),substring(substring-before(substring-after(substring-after(substring-after(substring-after(normalize-space(COL6),' '),' '),' '),' '),' '),1,1))"/>
			</xsl:variable>

			<xsl:variable name="ExpiryMonth">
				<!--<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(normalize-space(COL31),' '),' '),' '),' ')"/>-->
				<xsl:value-of select="substring-before(substring-after(normalize-space(COL6),' '),' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<!--<xsl:value-of select="substring(substring-before(substring-after(substring-after(substring-after(substring-after(normalize-space(COL31),' '),' '),' '),' '),' '),2,1)"/>-->
				<xsl:value-of select="substring(substring-before(substring-after(substring-after(normalize-space(COL6),' '),' '),' '),2,1)"/>
			</xsl:variable>
			<xsl:variable name="PutOrCall">
				<xsl:value-of select="substring(substring-before(COL6,' '),1,1)"/>
				<!--<xsl:value-of select="substring(substring-before(substring-after(substring-after(normalize-space(COL31),' '),' '),' '),1,1)"/>-->
			</xsl:variable>

			<xsl:variable name="StrikePrice">
				<!--<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL6),' '),' '),' '),' '),' '),' '),' '),' '),' ')"/>-->

				<xsl:value-of select="substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL6),' '),' '),' '),' '),' '),' ')"/>

			</xsl:variable>

			<xsl:variable name="MonthCode">
				<xsl:call-template name="MonthCodesVar">
					<xsl:with-param name="varMonth" select="$ExpiryMonth"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:value-of select="concat($UnderlyingSymbol,' ',$MonthCode,$ExpiryYear,$PutOrCall,$StrikePrice)"/>
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

			<xsl:for-each select ="//PositionMaster">


				<xsl:variable name="NetPosition">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL5"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($NetPosition) and COL2!='MISC.' and COL2!='CURRENCY' ">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'FUCHS'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL6"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>



						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="contains(COL2,'OPTION')">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:when test="contains(COL2,'FUTFOP')">
									<xsl:value-of select="'FutureOption'"/>
								</xsl:when>
								<xsl:when test="contains(COL2,'FUTURE')">
									<xsl:value-of select="'Future'"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="Symbol">

							<xsl:choose>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:call-template name ="Option">
										<xsl:with-param name="Symbol" select="COL6"/>
										<xsl:with-param name="Suffix" select="''"/>
									</xsl:call-template>
								</xsl:when>



								<!--<xsl:when test="$Asset='FuturesOption'">
									<xsl:call-template name ="FutureOption">
										<xsl:with-param name="Symbol" select="COL31"/>
										<xsl:with-param name="Suffix" select="''"/>
									</xsl:call-template>
								</xsl:when>-->

							</xsl:choose>
						</xsl:variable>





						<xsl:variable name="FutureOpt">
							<xsl:choose>
								<xsl:when test="$Asset='FutureOption'">
									<xsl:choose>
										<xsl:when test="contains(COL7,'1NQ4W3990N6')">
											<xsl:call-template name ="FutureOption">
												<xsl:with-param name="Symbol" select="COL6"/>
												<xsl:with-param name="Suffix" select="''"/>
											</xsl:call-template>
										</xsl:when>
										<xsl:otherwise>
											<xsl:call-template name ="FuturesOption">
												<xsl:with-param name="Symbol" select="COL6"/>
												<xsl:with-param name="Suffix" select="''"/>
											</xsl:call-template>
											<!--<xsl:value-of select="concat(substring(COL7,1,2),' ',substring(COL7,7,2),substring(COL7,3,4))"/>-->
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>



						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Asset='EquityOption'">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>

								<xsl:when test="$Asset='Future'">
									<xsl:value-of select="concat(substring(COL7,1,2),' ',substring(COL7,3,2))"/>
								</xsl:when>


								<xsl:when test="$Asset='FutureOption'">
									<xsl:value-of select="$FutureOpt"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>

						</Symbol>


						<xsl:variable name="PB_FUND_NAME" select="COL1"/>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<FundName>
							<xsl:choose>

								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</FundName>

						<NetPosition>
							<xsl:choose>
								<xsl:when test="$NetPosition &gt; 0">
									<xsl:value-of select="$NetPosition"/>
								</xsl:when>
								<xsl:when test="$NetPosition &lt; 0">
									<xsl:value-of select="$NetPosition* (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:variable name="CostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="''"/>
							</xsl:call-template>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>
								<xsl:when test="$CostBasis &gt; 0">
									<xsl:value-of select="$CostBasis"/>

								</xsl:when>
								<xsl:when test="$CostBasis&lt; 0">
									<xsl:value-of select="$CostBasis * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CostBasis>


						<xsl:variable name="Side" select="COL4"/>

						<SideTagValue>

							<xsl:choose>
								<xsl:when test ="$Asset='Option'">
									<xsl:choose>
										<xsl:when test ="$Side='BUY'">
											<xsl:value-of select ="'A'"/>
										</xsl:when>
										<xsl:when test ="$Side='SELL'">
											<xsl:value-of select ="'D'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select ="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test ="$Side='BUY'">
											<xsl:value-of select ="'1'"/>
										</xsl:when>
										<xsl:when test ="$Side='SELL'">
											<xsl:value-of select ="'2'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>


						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>


						<xsl:variable name ="Date" select="COL3"/>


						<PositionStartDate>
							<xsl:value-of select="$Date"/>
						</PositionStartDate>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>