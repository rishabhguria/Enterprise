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
				<xsl:when test="$Month=01 ">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month=02 ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=03 ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=04 ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=05 ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=06 ">
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
				<xsl:when test="$Month=01 ">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=02 ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=03 ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=04 ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=05 ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=06 ">
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

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="contains(substring(COL1,40,2),'EO')">
			<xsl:variable name="Symbols" select="substring(COL1,10,30)"/>
				
			
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="normalize-space(substring(COL1,10,4))"/>
			</xsl:variable>
			<xsl:variable name="varExDay">
				<xsl:value-of select="substring(substring-after($Symbols,'/'),1,2)"/>
			</xsl:variable>
			<xsl:variable name="varMonth">
				<!--<xsl:value-of select="substring(substring-before($Symbols,'/'),0)"/>-->
				<!--<xsl:value-of select="substring-after(substring-before(normalize-space($Symbols),'/'),' ')"/>-->
				<xsl:value-of select="substring(substring-after(substring-after(normalize-space($Symbols),' '),' '),1,2)"/>
				
			</xsl:variable>
			<xsl:variable name="varYear">
				<xsl:value-of select="substring(substring-after(substring-after($Symbols,'/'),'/'),1,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-after(substring-after(substring-after($Symbol,' '),' '),' '),1,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<!--<xsl:value-of select="substring(substring-after(substring-after(substring-after(normalize-space($Symbol),' '),' '),' '),2)"/>-->
				<xsl:value-of select="format-number(substring(substring-after(substring-after(substring-after(normalize-space($Symbol),' '),' '),' '),2),'#.00') "/>
			</xsl:variable>


			<xsl:variable name="MonthCodeVar">
				<xsl:call-template name="MonthCode">
					<xsl:with-param name="Month" select="number($varMonth)"/>
					<xsl:with-param name="PutOrCall" select="$PutORCall"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="Day">
				<xsl:choose>
					<xsl:when test="substring($varExDay,1,1)='0'">
						<xsl:value-of select="substring($varExDay,2,1)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$varExDay"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="ThirdFriday">
				<xsl:choose>
					<xsl:when test="number($varMonth) and number($varYear)">
						<xsl:value-of select="my:Now(number($varYear),number($varMonth))"/>
					</xsl:when>
				</xsl:choose>
			</xsl:variable>
			<!--<xsl:choose>
				<xsl:when test="number($ExpiryMonth)=1 and $ExpiryYear='15'">

					
				</xsl:when>

				<xsl:otherwise>-->
			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$varYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
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

			<xsl:for-each select ="//PositionMaster">
				
				<xsl:variable name="varPosition">
					<xsl:choose>
						<xsl:when test="substring(COL1,70,1)='M'">
							<xsl:value-of select="(substring(COL1,69,1)) *100000"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="substring(COL1,69,12)"/> 
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>


				<xsl:variable name="NetPosition">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="$varPosition"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($NetPosition) ">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'WH'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="substring(COL1,10,30)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						
						<xsl:variable name="Asset">
                       <xsl:choose>

                <xsl:when test="contains(substring(COL1,40,2),'EO')">
                  <xsl:value-of select="'Option'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>


						<xsl:variable name="Symbol">
							<xsl:value-of select="substring(COL1,112,6)"/>
						</xsl:variable>

						<Symbol>


							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$Asset='Option'">
									<xsl:call-template name ="Option">
										<xsl:with-param name="Symbol" select="substring(COL1,10,30)"/>
										<xsl:with-param name="Suffix" select="''"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:when test="$Asset='Equity'">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>

						</Symbol>


						<xsl:variable name="PB_FUND_NAME" select="substring(COL1,1,9)"/>

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


						<xsl:variable name ="CostDivid">
							<xsl:choose>
								<xsl:when test="contains(substring(COL1,58,11),'/')">
									<xsl:value-of select="substring-before(substring-after(substring(COL1,58,11),' '),'/') div substring-after(substring-after(substring(COL1,58,11),' '),'/')"/>
							
								</xsl:when>
						</xsl:choose>
						</xsl:variable>
						
						


						<xsl:variable name="CostBasis">
							<xsl:choose>
								<xsl:when test="contains(substring(COL1,58,11),'/')">
									
									<xsl:value-of select="number(substring(COL1,58,3)) +$CostDivid"/>
									
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="(substring(COL1,58,11))"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


 
						<CostBasis>
							<xsl:choose>
								<xsl:when test="$CostBasis &gt; 0">
									<xsl:value-of select="$CostBasis"/>

								</xsl:when>
								<xsl:when test="$CostBasis &lt; 0">
									<xsl:value-of select="$CostBasis * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CostBasis>







						<xsl:variable name="Side" select="substring(COL1,54,1)"/>

						<SideTagValue>

							<xsl:choose>

								<xsl:when test="$Side='B'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$Side='S'">
									<xsl:value-of select="2"/>
								</xsl:when>

								<xsl:when test="$Side='H'">
									<xsl:value-of select="'5'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>


							</xsl:choose>

						</SideTagValue>



						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>


						<xsl:variable name ="Date" select="substring(COL1,43,11)"/>


						<xsl:variable name="Year1" select="substring(substring-after(substring-after($Date,'/'),'/'),1,2)"/>
						<xsl:variable name="Months" select="substring(substring-before($Date,'/'),1,2)"/>
						<xsl:variable name="Day" select="substring-before(substring-after($Date,'/'),'/')"/>



						<PositionStartDate>

							<xsl:value-of select="concat($Months,'/',$Day,'/',$Year1)"/>
							
						</PositionStartDate>


					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>