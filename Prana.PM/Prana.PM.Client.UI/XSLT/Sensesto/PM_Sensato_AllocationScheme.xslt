<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">

        <xsl:if test ="COL1 != 'Ignore' ">


          <PositionMaster>

            <FundName>
              <xsl:value-of select="COL5"/>
            </FundName>

            <LongName>
              <xsl:value-of select ="COL9"/>
            </LongName>

            <SEDOL>
              <xsl:value-of select ="COL10"/>
            </SEDOL>

            <Bloomberg>
              <xsl:value-of select ="COL11"/>
            </Bloomberg>

            <Quantity>
              <xsl:value-of select ="COL13"/>
            </Quantity>

            <RoundLot>
              <xsl:value-of select ="COL14"/>
            </RoundLot>
          
             <AllocationBasedOn>
              <xsl:value-of select ="'SEDOL'"/>
            </AllocationBasedOn>

            <OrderSideTagValue>
              <xsl:choose>
                <xsl:when test="COL12 = 'BC'">
                  <xsl:value-of select="'A'"/>
                </xsl:when>
                <xsl:when test="COL12 = 'BL'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="COL12 = 'SS'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
                <xsl:when test="COL12 = 'SL'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </OrderSideTagValue>

            <TradeType>
              <xsl:value-of select ="COL6"/>
            </TradeType>

            <Currency>
              <xsl:value-of select ="COL7"/>
            </Currency>

            <PB>
              <xsl:value-of select ="COL4"/>
            </PB>

            <AllocationSchemeKey>
              <xsl:value-of select ="'PBSymbolSide'"/>
            </AllocationSchemeKey>

            <!--<SMMappingReq>
              <xsl:value-of select="'SecMasterMapping.xml'"/>
            </SMMappingReq>-->

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>


