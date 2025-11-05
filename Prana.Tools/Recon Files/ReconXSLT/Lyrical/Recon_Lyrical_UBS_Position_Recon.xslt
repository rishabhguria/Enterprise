<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select ="//Comparision">
				<xsl:variable name = "PB_NAME">
					<xsl:value-of select="'UBS'"/>
				</xsl:variable>

				<xsl:variable name = "PB_FUND_NAME">
					<xsl:value-of select="substring(COL1,1,7)"/>
				</xsl:variable>

				<xsl:variable name ="PRANA_FUND_NAME">
					<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
				</xsl:variable>

				<xsl:variable name ="varQuantity">
					<xsl:value-of select ="number(substring(COL1,18,14))"/>
				</xsl:variable>

				<xsl:if test ="$PRANA_FUND_NAME!='' and number($varQuantity) and (substring(COL1,8,6)!='919136' and substring(COL1,8,6)!='919446' and substring(COL1,8,6)!='919195'and substring(COL1,8,6)!='919101'and  substring(COL1,8,4)!='CASH')  and substring(COL1,32,9) !='90265M102'  and substring(COL1,32,9) !='751990441'  and substring(COL1,32,9) !='90265L104' ">

					<PositionMaster>

					

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="substring(COL1,32,9)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<!--<xsl:variable name="Asset" select="COL4"/>-->

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<CUSIP>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</CUSIP>

						<SMRequest>
							<xsl:value-of select="'true'"/>
						</SMRequest>



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

						<Quantity>
							<xsl:choose>
								<xsl:when  test="number($varQuantity) &gt; 0">
									<xsl:value-of select="$varQuantity"/>
								</xsl:when>
								<xsl:when test="number($varQuantity) &lt; 0">
									<xsl:value-of select="$varQuantity* (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<xsl:variable name="varAvgPrice">
							<xsl:value-of select="0"/>
						</xsl:variable>

						<AvgPX>
							<xsl:choose>
								<xsl:when test ='number($varAvgPrice) &lt; 0'>
									<xsl:value-of select ="$varAvgPrice*-1"/>
								</xsl:when>
								<xsl:when test ='number($varAvgPrice) &gt; 0'>
									<xsl:value-of select ='$varAvgPrice'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>

						<xsl:variable name="varSide" select="COL5"/>

						<Side>
							<xsl:choose>
								<xsl:when  test="number($varQuantity) &gt; 0">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="number($varQuantity) &lt; 0">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>

