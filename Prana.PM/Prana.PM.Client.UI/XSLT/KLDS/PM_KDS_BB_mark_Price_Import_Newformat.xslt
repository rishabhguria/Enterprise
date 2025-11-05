<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="MonthCode">
		<xsl:param name="varMonth"/>
		<xsl:choose>
			<xsl:when test="$varMonth=1">
				<xsl:value-of select="'F'"/>
			</xsl:when>
			<xsl:when test="$varMonth=2">
				<xsl:value-of select="'G'"/>
			</xsl:when>
			<xsl:when test="$varMonth=3">
				<xsl:value-of select="'H'"/>
			</xsl:when>
			<xsl:when test="$varMonth=4">
				<xsl:value-of select="'J'"/>
			</xsl:when>
			<xsl:when test="$varMonth=5">
				<xsl:value-of select="'K'"/>
			</xsl:when>
			<xsl:when test="$varMonth=6">
				<xsl:value-of select="'M'"/>
			</xsl:when>
			<xsl:when test="$varMonth=7">
				<xsl:value-of select="'N'"/>
			</xsl:when>
			<xsl:when test="$varMonth=8">
				<xsl:value-of select="'Q'"/>
			</xsl:when>
			<xsl:when test="$varMonth=9">
				<xsl:value-of select="'U'"/>
			</xsl:when>
			<xsl:when test="$varMonth=10">
				<xsl:value-of select="'V'"/>
			</xsl:when>
			<xsl:when test="$varMonth=11">
				<xsl:value-of select="'X'"/>
			</xsl:when>
			<xsl:when test="$varMonth=12">
				<xsl:value-of select="'Z'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


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

				<xsl:variable name="COL12">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL12"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:variable name ="MarkPrice">

					<xsl:choose>
						<xsl:when test="contains(COL1,'/') and (COL13='*' or COL13='')">
							<xsl:value-of select="COL12"/>
						</xsl:when>
						<xsl:when test="contains(COL1,'/')">
							<xsl:value-of select="COL13"/>
						</xsl:when>
						<xsl:when test="contains(COL1,'Equity')">
							<xsl:value-of select="$COL12"/>
						</xsl:when>
						<xsl:when test="contains(substring-after(normalize-space(COL1),' '),'Govt')">
							<xsl:value-of select="$COL12"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="COL8"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="markprice1" >
					<xsl:choose>
						<xsl:when test="string-length(substring-before(COL1,' '))=3 and contains(substring-after(COL1,' '),'CURNCY')">
							<xsl:value-of select="'false'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'true'"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:if test ="number($MarkPrice) and $markprice1='true'">

					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'SC'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL1"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_ROOT_NAME">
							<xsl:value-of select="substring(COL1,1,2)"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_ROOT_NAME">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode = $PB_ROOT_NAME]/@UnderlyingCode"/>
						</xsl:variable>

						<xsl:variable name ="FUTURE_EXCHANGE_CODE">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME]/@ExchangeCode"/>
						</xsl:variable>

						<xsl:variable  name="FUTURE_FLAG">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME]/@ExpFlag"/>
						</xsl:variable>

						<xsl:variable name="MonthCode">
							<xsl:value-of select="substring(COL1,3,1)"/>
						</xsl:variable>

						<xsl:variable name="Year" select="substring(COL1,4,1)"/>

						<xsl:variable name="MonthYearCode">
							<xsl:choose>
								<xsl:when test="$FUTURE_FLAG!=''">
									<xsl:value-of select="concat($Year,$MonthCode)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat($MonthCode,$Year)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="Underlying">
							<xsl:choose>
								<xsl:when test="$PRANA_ROOT_NAME!=''">
									<xsl:value-of select="$PRANA_ROOT_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_ROOT_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<Symbol>
							
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="COL1= 'SPY Equity'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="COL1= 'VEA Equity'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="COL1!='*' or COL1!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<Bloomberg>
							
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="COL1= 'VEA Equity'">
									<xsl:value-of select="'VEA US EQUITY'"/>
								</xsl:when>
								<xsl:when test="COL1= 'SPY Equity'">
									<xsl:value-of select="'SPY US EQUITY'"/>
								</xsl:when>
								
								<xsl:when test="COL1!='*' or COL1!=''">
									<xsl:value-of select="translate(COL1,$lower_CONST,$upper_CONST)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</Bloomberg>

						<Date>
							<xsl:value-of select="''"/>
						</Date>

						<xsl:variable name ="PRANA_Multiplier">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode = $PB_ROOT_NAME]/@PriceMul"/>
						</xsl:variable>

						<xsl:variable name="Markprice">
							<xsl:choose>

								<xsl:when test ="$MarkPrice &lt;0">
									<xsl:value-of select ="$MarkPrice * -1"/>
								</xsl:when>

								<xsl:when test ="$MarkPrice &gt;0">
									<xsl:value-of select ="$MarkPrice"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</xsl:variable>

						<MarkPrice>

							<xsl:choose>
								<xsl:when test="contains(COL1,'/') and (COL13='*' or COL13='')">
									<xsl:value-of select="COL12"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'/')">
									<xsl:value-of select="COL13"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'Equity') or contains(COL1,'Govt')">
									<xsl:value-of select="COL12"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$PRANA_Multiplier!=''">
											<xsl:value-of select="COL8 * $PRANA_Multiplier"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="COL8"/>
										</xsl:otherwise>
									</xsl:choose>

								</xsl:otherwise>


							</xsl:choose>


						</MarkPrice>

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>


	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>

