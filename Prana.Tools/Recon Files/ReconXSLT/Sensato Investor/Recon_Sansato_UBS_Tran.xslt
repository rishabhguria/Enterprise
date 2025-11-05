<?xml version="1.0" encoding="UTF-8"?>
										<!-- Object -Trade Recon for UBS Date -01-11-2012(dd/MM/yyyy)-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:user="http://www.contoso.com">

	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//Comparision">
				<xsl:if test="COL1 != 'Account Name'">
					<PositionMaster>

						<!--COL For Fund -->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL6"/>
						</xsl:variable>
						
						<!--Fund Mapping -->
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SANSATO']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<FundName>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose>
						
						<xsl:variable name="PB_COMPANY_NAME" select="COL11"/>

						<PBSymbol>
							<xsl:value-of select="COL10"/>
						</PBSymbol>

						<!--NEED TO ADD HERE pb name FROM SymbolMapping.xml  -->
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='SANSATO']/SymbolData[@CompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>
						
						<CompanyName>
							<xsl:value-of select="$PB_COMPANY_NAME"/>
						</CompanyName>

						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME !=''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>
								<SEDOL>
									<xsl:value-of select="''"/>
								</SEDOL>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<SEDOL>
									<xsl:value-of select="COL10"/>
								</SEDOL>
							</xsl:otherwise>
						</xsl:choose>

						
						<xsl:choose>
							<xsl:when test ="contains(COL8,'(') != false ">
								<Side>
									<xsl:value-of select="substring-before(COL8,'(')"/>
								</Side>
							</xsl:when>							
							<xsl:otherwise>
								<Side>
									<xsl:value-of select="''"/>
								</Side>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when  test="number(COL14) and COL14 &lt; 0">
								<Quantity>
									<xsl:value-of select="COL14 * (-1)"/>
								</Quantity>
							</xsl:when>
							<xsl:when  test="number(COL14) and COL14 &gt; 0">
								<Quantity>
									<xsl:value-of select="COL14"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="'0'"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>

						<!--Need To Ask Here............-->
						<xsl:choose>
							<xsl:when test="number(COL16) and  number(COL16) &lt; 0">
								<AvgPX>
									<xsl:value-of select= "COL16 *(-1)"/>
								</AvgPX>
							</xsl:when>
							<xsl:when test="number(COL16) and  number(COL16) &gt; 0">
								<AvgPX>
									<xsl:value-of select= "COL16"/>
								</AvgPX>
							</xsl:when>
							<xsl:otherwise>
								<AvgPX>
									<xsl:value-of select= "0"/>
								</AvgPX>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="number(COL17) and COL17 &lt; 0">
								<Commission>
									<xsl:value-of select="COL17 * (-1)"/>
								</Commission>
							</xsl:when>
							<xsl:when test="number(COL17) and COL17 &gt; 0">
								<Commission>
									<xsl:value-of select="COL17"/>
								</Commission>
							</xsl:when>
							<xsl:otherwise>
								<Commission>
									<xsl:value-of select="0"/>
								</Commission>
							</xsl:otherwise>
						</xsl:choose>


						<Fees>
							<xsl:value-of select="0"/>
						</Fees>

						<!--GROSS NOTIONAL-->

						<GrossNotionalValue>
							<xsl:value-of select="0"/>
						</GrossNotionalValue>

						<!--NET NOTIONAL-->
						<xsl:choose>
							<xsl:when test="number(COL18) and COL18 &lt; 0">
								<NetNotionalValue>
									<xsl:value-of select="COL18 * (-1)"/>
								</NetNotionalValue>
							</xsl:when>
							<xsl:when test="number(COL18) and COL18 &gt; 0">
								<NetNotionalValue>
									<xsl:value-of select="COL18"/>
								</NetNotionalValue>
							</xsl:when>
							<xsl:otherwise>
								<NetNotionalValue>
									<xsl:value-of select="0"/>
								</NetNotionalValue>
							</xsl:otherwise>
						</xsl:choose>

						<SMRequest>
							<xsl:value-of select ="'TRUE'"/>
						</SMRequest>

					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
