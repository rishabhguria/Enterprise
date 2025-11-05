<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

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

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:if test="normalize-space(COL7)='Future'">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Nokomis'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL2)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<!--<xsl:variable name="Underlying">
							<xsl:value-of select="string-length(substring-before(COL24,' '))-2"/>
						</xsl:variable>-->

						<xsl:variable name="PB_ROOT">
							<xsl:value-of select="substring(COL1,1,2)"/>
						</xsl:variable>

						<xsl:variable name="PB_ROOT_NAME">
							<xsl:value-of select="normalize-space($PB_ROOT)"/>
						</xsl:variable>

						<xsl:variable name="PB_YELLOW_NAME">
							<!--<xsl:value-of select="normalize-space()"/>-->
							<xsl:choose>
								<xsl:when test ="contains(substring-after(normalize-space(COL1),' '),' ')">
									<xsl:value-of select="substring-after(substring-after(normalize-space(COL1),' '),' ')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-after(normalize-space(COL1),' ')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<!--<xsl:variable name="PB_EXCHANGE_NAME">
							<xsl:value-of select="normalize-space(COL91)"/>
						</xsl:variable>-->

						<xsl:variable name ="PRANA_ROOT_NAME">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode = $PB_ROOT_NAME and @YellowFlag = $PB_YELLOW_NAME]/@UnderlyingCode"/>
						</xsl:variable>

						<xsl:variable name ="FUTURE_EXCHANGE_CODE">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME and @YellowFlag= $PB_YELLOW_NAME]/@ExchangeCode"/>
						</xsl:variable>

						<xsl:variable  name="FUTURE_FLAG">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME and @YellowFlag= $PB_YELLOW_NAME]/@ExpFlag"/>
						</xsl:variable>

						<xsl:variable name="MonthCode">
							<!--<xsl:call-template name="MonthCode">
								<xsl:with-param name="varMonth" select="number(substring-before(substring-after(normalize-space(COL18),'/'),'/'))"/>
							</xsl:call-template>-->
							<xsl:value-of select="substring(normalize-space(COL1),3,1)"/>
						</xsl:variable>

						<xsl:variable name="Year" select="substring(normalize-space(COL1),4,1)"/>

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
									<xsl:value-of select="translate($PRANA_ROOT_NAME,$lower_CONST,$upper_CONST)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="translate($PB_ROOT,$lower_CONST,$upper_CONST)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<TickerSymbol>

							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="translate($PRANA_SYMBOL_NAME,$lower_CONST,$upper_CONST)"/>
								</xsl:when>

								<!--<xsl:when test="normalize-space(COL7)='Future'">
									<xsl:value-of select="normalize-space(concat($Underlying,$MonthYearCode))"/>
								</xsl:when>-->

								<xsl:when test="normalize-space(COL7)='Future'">
									<xsl:value-of select="normalize-space(COL9)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="translate($PB_SYMBOL_NAME,$lower_CONST,$upper_CONST)"/>
								</xsl:otherwise>

							</xsl:choose>

						</TickerSymbol>

						<BloombergSymbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="normalize-space(COL7)='Future'">
									<xsl:value-of select="translate(normalize-space(COL1),$lower_CONST,$upper_CONST)"/>
								</xsl:when>

								<xsl:otherwise>
									<!--<xsl:value-of select="translate($PB_SYMBOL_NAME,$lower_CONST,$upper_CONST)"/>-->
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</BloombergSymbol>
						
						<ExpirationDate>
							<xsl:value-of select="normalize-space(COL4)"/>
						</ExpirationDate>

						<xsl:variable name="Currency" select="normalize-space(COL8)"/>

						<xsl:variable name="Exchange" select="normalize-space(COL6)"/>

						<AUECID>
							<xsl:choose>
								<xsl:when test="$Currency='AUD'">
									<xsl:value-of select="91"/>
								</xsl:when>
								<xsl:when test="$Currency='EUR'">
									<xsl:value-of select="93"/>
								</xsl:when>
								<xsl:when test="$Currency='GBP'">
									<xsl:value-of select="92"/>
								</xsl:when>
								<xsl:when test="$Currency='CAD'">
									<xsl:value-of select="89"/>
								</xsl:when>
								<xsl:when test="$Currency='JPY'">
									<xsl:choose>
										<xsl:when test="$Exchange='SGX'">
											<xsl:value-of select="90"/>
										</xsl:when>
										<xsl:when test="$Exchange='TSE-Fut'">
											<xsl:value-of select="23"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:when test="$Currency='USD'">
									<xsl:choose>
										<xsl:when test="$Exchange='CME'">
											<xsl:value-of select="16"/>
										</xsl:when>
										<xsl:when test="$Exchange='CBT'">
											<xsl:value-of select="86"/>
										</xsl:when>
										<xsl:when test="$Exchange='CMX'">
											<xsl:value-of select="85"/>
										</xsl:when>
										<xsl:when test="$Exchange='ICE' or $Exchange='NYB' or $Exchange='FNX'">
											<xsl:value-of select="100"/>
										</xsl:when>
										<xsl:when test="$Exchange='NYM'">
											<xsl:value-of select="84"/>
										</xsl:when>
										<xsl:when test="$Exchange='MGE'">
											<xsl:value-of select="17"/>
										</xsl:when>
										
										
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AUECID>

						<LongName>
							<xsl:value-of select="translate(translate(translate($PB_SYMBOL_NAME,$lower_CONST,$upper_CONST),'$',''),'£','')"/>
						</LongName>

						<UnderLyingSymbol>
							<!--<xsl:value-of select="normalize-space($Underlying)"/>-->
							<xsl:value-of select="normalize-space(COL9)"/>
						</UnderLyingSymbol>


						<xsl:variable name="COL5">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL5"/>
							</xsl:call-template>
						</xsl:variable>

						<!--<xsl:variable name="COL49">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL49"/>
							</xsl:call-template>
						</xsl:variable>-->

						<xsl:variable name="Multiplier">
							<xsl:choose>
								<xsl:when test="number($COL5)">
									<xsl:value-of select="$COL5"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Multiplier>
							<xsl:choose>
								<xsl:when test="$Multiplier &gt; 0">
									<xsl:value-of select="$Multiplier"/>

								</xsl:when>
								<xsl:when test="$Multiplier &lt; 0">
									<xsl:value-of select="$Multiplier * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>

							</xsl:choose>
						</Multiplier>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>