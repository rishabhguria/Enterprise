<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
	<DocumentElement>
		<xsl:for-each select="//PositionMaster">
			<xsl:if test="number(COL3)">
			<PositionMaster>

				<xsl:variable name="PB_CountnerParty" select="COL1"/>

				<xsl:variable name="PRANA_CounterPartyID">
					<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name= 'GS']/BrokerData[@PBBroker=$PB_CountnerParty]/@PranaBrokerID"/>
				</xsl:variable>

				<xsl:variable name ="varFut">
					<xsl:value-of select ="substring(COL1, (string-length(COL1)-2), 3)"/>
				</xsl:variable>

				<AccountName>
					<xsl:value-of select="''"/>
				</AccountName>

				<PositionStartDate>
					<xsl:value-of select="''"/>
				</PositionStartDate>

				<CounterPartyID>
					<!--<xsl:choose>
						<xsl:when test="COL1='C'">-->

					<xsl:choose>
						<xsl:when test ="$varFut = 'FUT'">
							<xsl:value-of select="13"/>
						</xsl:when>
						<xsl:when test ="substring-after(COL4, ' ') = 'US'">
							<xsl:value-of select="17"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="15"/>
						</xsl:otherwise>
					</xsl:choose>
				</CounterPartyID>

				<SideTagValue>
					<xsl:choose>
						<xsl:when test="translate(COL2, $vLowercaseChars_CONST, $vUppercaseChars_CONST)='BUY'">
							<xsl:value-of select="'1'"/>
						</xsl:when>
						<xsl:when test="COL2= 'COVER'">
							<xsl:value-of select="'B'"/>
						</xsl:when>
						<xsl:when test="translate(COL2, $vLowercaseChars_CONST, $vUppercaseChars_CONST)='SELL'">
							<xsl:value-of select="'2'"/>
						</xsl:when>
						<xsl:when test="COL2= 'SHORT'">
							<xsl:value-of select="'5'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="''"/>
						</xsl:otherwise>
					</xsl:choose>
				</SideTagValue>

				<NetPosition>
					<xsl:choose>
						<xsl:when  test="number(normalize-space(COL3)) &gt; 0">
							<xsl:value-of select="COL3"/>
						</xsl:when>
						<xsl:when test="number(normalize-space(COL3)) &lt; 0">
							<xsl:value-of select="COL3* (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</NetPosition>

				<xsl:variable name="PBSuffixCode">
					<xsl:value-of select = "COL4"/>
				</xsl:variable>

				<xsl:variable name="PB_ExchangeCODE">
					<xsl:value-of select="substring-after(COL4, ' ')"/>
				</xsl:variable>

				<xsl:variable name="PRANA_Exchange">
					<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name='JPM']/SymbolData[@PBSuffixCode=$PB_ExchangeCODE]/@PranaSuffixCode"/>
				</xsl:variable>

				<xsl:variable name ="PB_SymbolName">
					<xsl:value-of select ="substring-before(COL4, ' ')"/>
				</xsl:variable>

				<xsl:variable name="PB_Symbol" select="COL4"/>
				<xsl:variable name="PRANA_SYMBOL_NAME">
					<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name= 'JPM']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
				</xsl:variable>

				<xsl:variable name ="varBBCode">
					<xsl:value-of select ="normalize-space(substring(COL4, 1,2))"/>
				</xsl:variable>

				<xsl:variable name ="varBBKey">
					<xsl:value-of select ="normalize-space(substring(COL4, 6))"/>
				</xsl:variable>

				

				<xsl:variable name="PRANA_ROOT_NAME">
					<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name='JPM']/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@UnderlyingCode"/>
				</xsl:variable>

				<xsl:variable name="PRANA_EXCHANGE_NAME">
					<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name='JPM']/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@ExchangeName"/>				
				</xsl:variable>
				
				<Symbol>
					<xsl:choose>
						<xsl:when test="$PRANA_SYMBOL_NAME !=''">
							<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
						</xsl:when>
						<xsl:when test ="$varFut = 'FUT'">
							<xsl:value-of select ="concat($PRANA_ROOT_NAME, ' ', substring(COL4,3,2), $PRANA_EXCHANGE_NAME)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="translate(concat($PB_SymbolName,$PRANA_Exchange),'/','.')"/>
						</xsl:otherwise>
					</xsl:choose>
				</Symbol>				

				<xsl:variable name="PB_GrossPrice" select="COL6"/>

				<xsl:variable name="PB_Quantity" select="COL3"/>

				<CostBasis>
					<xsl:choose>
						<xsl:when test ="$PRANA_Exchange = '-LON' or $PRANA_Exchange = '-JSE' or $PRANA_Exchange = '-TAE'">
							<xsl:value-of select ="COL9 div 100"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="COL9"/>
						</xsl:otherwise>
					</xsl:choose>
				</CostBasis>

			</PositionMaster>
			</xsl:if>
		</xsl:for-each>

	</DocumentElement>
</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>


</xsl:stylesheet> 
