<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

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

				 
					<!--<PositionMaster>
 
						<AccountName>
							<xsl:value-of select="''"/>
						</AccountName>

					 
						<Symbol>
							<xsl:value-of select="''"/>
						</Symbol>

 
						<CostBasis>
							<xsl:value-of select="0"/>
						</CostBasis>

						<NetPosition>
							<xsl:value-of select="0"/>
						</NetPosition>


						<SideTagValue>
							<xsl:value-of select="''"/>

						</SideTagValue>

						<PositionStartDate>
							<xsl:value-of select="''"/>
						</PositionStartDate>


						<PBSymbol>
							<xsl:value-of select="''"/>
						</PBSymbol>
					</PositionMaster>-->
			
				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL6"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($Position)">
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Jone'"/>
						</xsl:variable>
						<xsl:variable name = "PB_COMPANY_NAME" >
							<xsl:value-of select="COL4"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="''"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<xsl:variable name="Symbol">
							<xsl:value-of select="COL2"/>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_COMPANY_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

					

						<xsl:variable name="CostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL7"/>
							</xsl:call-template>
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


						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="substring-after(substring-after(substring-after(substring-after(normalize-space(COL4),' '),' '),' '),' ') ='CALL' or substring-after(substring-after(substring-after(substring-after(normalize-space(COL4),' '),' '),' '),' ') ='PUT'">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						<xsl:variable name="Side">
							<xsl:value-of select="COL5"/>
						</xsl:variable>
						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$Asset ='EquityOption'">
									<xsl:choose>
										<xsl:when test="$Side ='Long'">
											<xsl:value-of select="'A'"/>
										</xsl:when>
										<xsl:when test="$Side ='Short'">
											<xsl:value-of select="'C'"/>
										</xsl:when>
										
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
									<xsl:when test="$Side='Long'">
										<xsl:value-of select="'1'"/>
									</xsl:when>
									<xsl:when test="$Side='Short'">
										<xsl:value-of select="'5'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>						
							
						</SideTagValue>
						
						<PositionStartDate>
							<xsl:value-of select="COL1"/>
						</PositionStartDate>
						

						<PBSymbol>
							<xsl:value-of select="$PB_COMPANY_NAME"/>
						</PBSymbol>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


