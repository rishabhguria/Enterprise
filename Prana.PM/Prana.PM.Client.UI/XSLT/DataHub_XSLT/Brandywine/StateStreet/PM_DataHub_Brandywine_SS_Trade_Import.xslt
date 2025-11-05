<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:my="put-your-namespace-uri-here">

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

	</xsl:template>

	
	<xsl:template match="/">

		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="substring(COL1,51,17)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Position)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'SS'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="varCusip" select="substring(COL1,1769,9)"/>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>


								<xsl:when test="$varCusip!=''">
									<xsl:value-of select="''"/>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<CUSIP>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>


								<xsl:when test="$varCusip!=''">
									<xsl:value-of select="$varCusip"/>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</CUSIP>



						<xsl:variable name="PB_FUND_NAME" select="normalize-space(substring(COL1,36,12))"/>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../../../MappingFiles/ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>



						<NetPosition>
							<xsl:choose>
								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="$Position"/>
								</xsl:when>
								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="$Position * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						
						
						<xsl:variable name="varCostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="substring(COL1,68,15)"/>
							</xsl:call-template>
						</xsl:variable>

						
						<CostBasis>
							<xsl:choose>
								<xsl:when test="$varCostBasis &gt; 0">
									<xsl:value-of select="$varCostBasis"/>

								</xsl:when>
								<xsl:when test="$varCostBasis &lt; 0">
									<xsl:value-of select="$varCostBasis * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						
						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>




						<xsl:variable name="varMonth">
							<xsl:value-of select="substring(COL1,16,2)"/>
						</xsl:variable>

						<xsl:variable name="varDay">
							<xsl:value-of select="substring(COL1,18,2)"/>
						</xsl:variable>

						<xsl:variable name="varYear">
							<xsl:value-of select="substring(COL1,12,4)"/>
						</xsl:variable>

						<PositionStartDate>							
							<xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
						</PositionStartDate>

					

						<PositionSettlementDate>
							
							<xsl:value-of select="''"/>
						</PositionSettlementDate>



						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="substring(COL1,99,15)"/>
							</xsl:call-template>
						</xsl:variable>
						<Commission>
							<xsl:choose>
								<xsl:when test="$Commission &gt; 0">
									<xsl:value-of select="$Commission"/>
								</xsl:when>
								<xsl:when test="$Commission &lt; 0">
									<xsl:value-of select="$Commission * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="FxRate">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="substring(COL1,253,19)"/>
							</xsl:call-template>
						</xsl:variable>
						<FxRate>
							<xsl:choose>
								<xsl:when test="number($FxRate)">
									<xsl:value-of select="$FxRate"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</FxRate>

						<xsl:variable name="varSecFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="substring(COL1,114,15)"/>
							</xsl:call-template>
						</xsl:variable>
						<SecFee>
							<xsl:choose>
								<xsl:when test="$varSecFee &gt; 0">
									<xsl:value-of select="$varSecFee"/>
								</xsl:when>
								<xsl:when test="$varSecFee &lt; 0">
									<xsl:value-of select="$varSecFee * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecFee>
						<xsl:variable name="varFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="format-number(substring(COL1,1844,15),'##.##')"/>
							</xsl:call-template>
						</xsl:variable>
						<Fees>
							<xsl:choose>
								<xsl:when test="$varFee &gt; 0">
									<xsl:value-of select="$varFee"/>
								</xsl:when>
								<xsl:when test="$varFee &lt; 0">
									<xsl:value-of select="$varFee * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>

						<xsl:variable name="MiscFees">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="format-number(substring(COL1,174,15),'##.##')"/>
							</xsl:call-template>
						</xsl:variable>
						<MiscFees>
							<xsl:choose>
								<xsl:when test="$MiscFees &gt; 0">
									<xsl:value-of select="$MiscFees"/>
								</xsl:when>
								<xsl:when test="$MiscFees &lt; 0">
									<xsl:value-of select="$MiscFees * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MiscFees>

						<CurrencySymbol>
							<xsl:value-of select="substring(COL1,48,3)"/>
						</CurrencySymbol>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>
</xsl:stylesheet>